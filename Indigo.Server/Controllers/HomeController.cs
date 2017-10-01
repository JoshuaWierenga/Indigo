using Indigo.Core.Models;
using Indigo.Server.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Indigo.Server.Controllers
{
    public class HomeController : Controller
    {
		private readonly IndigoContext _context;

		public HomeController(IndigoContext context)
		{
			_context = context;
		}

		public ActionResult Index()
        {
			int? currentUser = HttpContext.Session.GetInt32("CurrentUser");

			if (currentUser.HasValue)
			{
				return RedirectToAction("Details", "Users",  new { id = currentUser });
			}
            return View("Login");
        }

		public async Task<IActionResult> Login([Bind("Username,PasswordHash")] User user)
		{
			if (ModelState.IsValid)
			{
				var foundUser = await _context.Users
					.SingleOrDefaultAsync(m => m.Username == user.Username && m.PasswordHash == user.PasswordHash);

				if (foundUser != null)
				{
					HttpContext.Session.SetInt32("CurrentUser", foundUser.UserId);
				}
			}
			return RedirectToAction("Index");
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Remove("CurrentUser");

			return RedirectToAction("Index");
		}
	}
}