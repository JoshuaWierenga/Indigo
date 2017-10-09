using Indigo.Core.Models;
using Indigo.Server.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

		public async Task<ActionResult> Index()
        {
			int? currentUser = HttpContext.Session.GetInt32("CurrentUser");

			if (currentUser.HasValue)
			{
				var foundUser = await _context.Users
				.Include(u => u.UserConversations)
					.ThenInclude(uc => uc.Conversation)
						.ThenInclude(ucc => ucc.UserConversations)
							.ThenInclude(uccuc => uccuc.User)
				.SingleOrDefaultAsync(m => m.UserId == currentUser);

				return View(_context.UserConversations.Where(x => x.UserId == currentUser));
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

		public async Task<IActionResult> DeleteConversation(int id)
		{
			int? currentUser = HttpContext.Session.GetInt32("CurrentUser");

			if (currentUser.HasValue)
			{
				var conversation = await _context.Conversations
					.Include(c => c.UserConversations)
					.SingleOrDefaultAsync(c => c.ConversationId == id 
					&& c.UserConversations.SingleOrDefault(uc => uc.UserId == currentUser) != null);

				_context.Conversations.Remove(conversation);
				await _context.SaveChangesAsync();			
			}

			return RedirectToAction("Index");
		}
	}
}