using Microsoft.AspNetCore.Mvc;
using EvAldarado.Models;
using System.Linq;
using EvAldarado.DAL;
using System.Collections.Generic;

namespace EvAldarado.Controllers
{
    [Area("AdminIdare")]
    public class CategoryController : Controller
    {
        private readonly AppDBContext _context;
        public CategoryController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public JsonResult CategoryList()
        {
            return Json(_context.Categories.ToList());
        }

        [HttpPost]
        public JsonResult Add([FromBody] Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Json("Category added successfully.");
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return Json(category);
        }

        [HttpPost]
        public JsonResult Update([FromBody] Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return Json("Category updated successfully.");
        }

        [HttpPost]
        public JsonResult Activate(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                category.IsActive = true;
                _context.SaveChanges();
                return Json("Category activated successfully.");
            }
            return Json("Category not found.");
        }

        [HttpPost]
        public JsonResult Deactivate(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                category.IsActive = false;
                _context.SaveChanges();
                return Json("Category deactivated successfully.");
            }
            return Json("Category not found.");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return Json("Category deleted successfully.");
            }
            return Json("Category not found.");
        }
    }
}
