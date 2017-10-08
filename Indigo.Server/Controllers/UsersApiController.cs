using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Indigo.Core.Models;
using Indigo.Server.Context;

namespace Indigo.Server.Controllers
{
	[Produces("application/json")]
	[Route("api/users")]
	public class UsersApiController : Controller
	{
		private readonly IndigoContext _context;

		public UsersApiController(IndigoContext context)
		{
			_context = context;
		}

		// GET: api/users
		/// <summary>
		/// Takes username and password and checks if they exist and are correct
		/// and returns the full user object if they are
		/// </summary>
		/// <param name="Username">Username to check</param>
		/// <param name="PasswordHash">Password to check</param>
		/// <returns>Complete user object with user info, user's userconversations and the conversations for those</returns>
		[HttpGet]
		public async Task<IActionResult> GetUser([FromHeader] string Username, [FromHeader] string PasswordHash)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}


			if (Username == null || Username == "" ||
				PasswordHash == null || PasswordHash == "")
			{
				return BadRequest();
			}

			var foundUser = await _context.Users
				.Select(u => new
				{
					u.UserId,
					u.Username,
					u.PasswordHash,
					UserConversations = u.UserConversations.Select(uc => new
					{
						Conversation = new
						{
							uc.Conversation.ConversationId,
							uc.Conversation.ConversationName,
							uc.Conversation.isGroupChat,
							UserConversations = uc.Conversation.UserConversations
								.Where(uccuc => uccuc.User.Username != Username)
								.Select(uccuc => new
								{
									User = new
									{
										uccuc.User.Username
									},
									uccuc.isAdmin
								}),
							uc.isAdmin
						}
					})
				})
				.SingleOrDefaultAsync(u => u.Username == Username && u.PasswordHash == PasswordHash);

			if (foundUser == null)
			{
				return NotFound();
			}

			return Ok(foundUser);
		}
	}
}