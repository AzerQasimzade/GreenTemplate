using Microsoft.AspNetCore.Mvc;

namespace GreenSpecial.Areas.GreenAdmin.Controllers
{
	[Area("GreenAdmin")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
