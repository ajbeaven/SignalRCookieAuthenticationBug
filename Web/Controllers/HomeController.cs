using Microsoft.AspNetCore.Mvc;

namespace SignalRCookieAuthenticationBug.Web.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
