using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Indigo.Server.Models;
using System.Threading.Tasks;
using Indigo.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Indigo.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly IndigoContext _context;

        public HomeController(IndigoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string pagename)
        {
            Page foundPage = await _context.Pages.SingleOrDefaultAsync(p => p.Name == pagename);

            return View(foundPage != null ? foundPage : new Page
            {
                Name = pagename != null ? pagename : "home"          
            });
            
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}