using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EvAldarado.DAL;
using EvAldarado.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace EvAldarado.Controllers
{
    [Authorize] // Apply this attribute to ensure only logged-in users can access the actions
    public class UserProductsController : Controller
    {
        private readonly AppDBContext _appDBContext;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UserProductsController> _logger;
        private readonly UserManager<Users> _userManager;

        public UserProductsController(AppDBContext appDBContext, IWebHostEnvironment environment, ILogger<UserProductsController> logger, UserManager<Users> userManager)
        {
            _appDBContext = appDBContext;
            _environment = environment;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var userProducts = _appDBContext.UserProducts
                                             .Where(p => p.UserId == user.Id)
                                             .OrderByDescending(p => p.Id)
                                             .ToList();
            return View(userProducts);
        }

        public async Task<IActionResult> Show(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var product = _appDBContext.UserProducts
                                       .Where(p => p.UserId == user.Id)
                                       .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                AdvertisementName = product.AdvertisementName,
                AgentName = product.AgentName,
                Category = product.Category,
                SquareRoot = product.SquareRoot,
                DocumentType = product.DocumentType,
                Floor = product.Floor,
                Room = product.Room,
                Repaired = product.Repaired,
                DateBuilt = product.DateBuilt,
                Description = product.Description,
                Cost = product.Cost,
                AgentEmail = product.AgentEmail,
                AgentTelephone = product.AgentTelephone
            };
            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.Imagine; // Assuming 'Imagine' is the field that stores the image file name.

            return View(productDto);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            string baseImageFilename = SaveImageFile(productDto.BaseImageFile);

            UserProducts userProduct = new UserProducts
            {
                AdvertisementName = productDto.AdvertisementName,
                AgentName = productDto.AgentName,
                Category = productDto.Category,
                SquareRoot = productDto.SquareRoot,
                DocumentType = productDto.DocumentType,
                Floor = productDto.Floor,
                Room = productDto.Room,
                Repaired = productDto.Repaired,
                DateBuilt = productDto.DateBuilt,
                Description = productDto.Description,
                Cost = productDto.Cost,
                AgentEmail = productDto.AgentEmail,
                AgentTelephone = productDto.AgentTelephone,
                Imagine = baseImageFilename,
                UserId = user.Id // Set the user ID
            };

            if (productDto.imagesFiles != null && productDto.imagesFiles.Count > 0)
            {
                userProduct.Images = productDto.imagesFiles.Select(file => new Images
                {
                    ImgUrlbase = SaveImageFile(file)
                }).ToList();
            }

            _appDBContext.UserProducts.Add(userProduct);

            try
            {
                _appDBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the new product.");
                ModelState.AddModelError("", "An error occurred while saving the product. Please try again!");
                return View(productDto);
            }

            return RedirectToAction("Index");
        }

        private string SaveImageFile(IFormFile file)
        {
            if (file == null) return null;

            string newFilename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(_environment.WebRootPath, "Uploadsproducts", newFilename);

            using (var stream = System.IO.File.Create(fullPath))
            {
                file.CopyTo(stream);
            }

            return newFilename;
        }
    }
}
