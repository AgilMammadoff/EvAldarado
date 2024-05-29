using EvAldarado.DAL;
using EvAldarado.Models;
using EvAldarado.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PropertySearch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EvAldarado.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AppDBContext appDBContext;

        public HomeController(ILogger<HomeController> logger, AppDBContext _appDBContext)
        {
            appDBContext = _appDBContext;

            _logger = logger;
        }

		public ActionResult Index()
		{
			HomeViewModel model = new HomeViewModel
			{
				Sliders = appDBContext.Slider.ToList(),  
				UserProducts = appDBContext.UserProducts.ToList(),  
			};

			return View(model);
		}
		[HttpGet]
		public IActionResult Search(SearchParameters parameters)
		{
			// Replace this with actual search logic, e.g., querying a database
			var results = new List<SearchResult>
			{
				new SearchResult { Title = "Property 1", Location = parameters.Location, Price = parameters.MinPrice },
				new SearchResult { Title = "Property 2", Location = parameters.Location, Price = parameters.MaxPrice }
			};

			return View("Results", results);
		}
		public IActionResult About()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}