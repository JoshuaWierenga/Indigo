using Microsoft.AspNetCore.Mvc;

namespace Indigo.Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Login");
        }
    }
}