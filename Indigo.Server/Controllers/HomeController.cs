using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Indigo.Server.Models;

namespace Indigo.Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string id)
        {
            ViewData["Message"] = id;

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}