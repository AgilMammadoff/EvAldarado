using EvAldarado.DAL;
using EvAldarado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EvAldarado.Areas.AdminIdare.Controllers
{
    [Area("AdminIdare")]
    public class SliderController : Controller
    {
        private readonly AppDBContext _appDBContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderController(AppDBContext appDBContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDBContext = appDBContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var sliders = _appDBContext.Slider.ToList();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderDto sliderDto)
        {
            if (sliderDto.Image == null)
            {
                ModelState.AddModelError("Image", "Empty");
            }
            if (!ModelState.IsValid)
            {
                return View(sliderDto);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(sliderDto.Image.FileName);
            string imageFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploadSlider", newFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(imageFullPath));

            using (var stream = new FileStream(imageFullPath, FileMode.Create))
            {
                await sliderDto.Image.CopyToAsync(stream);
            }

            Slider slider = new Slider()
            {
                Name = sliderDto.Name,
                Description = sliderDto.Description,
                ImageUrl = newFileName
            };

            _appDBContext.Slider.Add(slider);
            await _appDBContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var slider = _appDBContext.Slider.FirstOrDefault(s => s.Id == id);
            if (slider == null)
            {
                return NotFound();
            }
            var sliderDto = new SliderDto
            {
                Name = slider.Name,
                Description = slider.Description
            };
            return View(sliderDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SliderDto sliderDto)
        {
            if (!ModelState.IsValid)
            {
                return View(sliderDto);
            }

            var slider = _appDBContext.Slider.FirstOrDefault(s => s.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            if (sliderDto.Image != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(sliderDto.Image.FileName);
                string imageFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploadSlider", newFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(imageFullPath));

                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await sliderDto.Image.CopyToAsync(stream);
                }

                slider.ImageUrl = newFileName;
            }

            slider.Name = sliderDto.Name;
            slider.Description = sliderDto.Description;

            _appDBContext.Slider.Update(slider);
            await _appDBContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var slider = _appDBContext.Slider.FirstOrDefault(s => s.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slider = _appDBContext.Slider.FirstOrDefault(s => s.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            _appDBContext.Slider.Remove(slider);
            await _appDBContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}