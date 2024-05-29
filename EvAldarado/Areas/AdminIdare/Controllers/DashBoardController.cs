using Microsoft.AspNetCore.Mvc;

namespace EvAldarado.Areas.AdminIdare.Controllers
{
	[Area ("AdminIdare")]
	public class DashBoardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
