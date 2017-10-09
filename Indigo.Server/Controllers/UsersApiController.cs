using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Indigo.Server.Context;
using Indigo.Core.Models;

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
		/// Requires user auth
		/// takes a username and attempts to find a user with it
		/// if the user is is auth user then their full user object is returned
		/// otherwise just the userid and username is returned
		/// </summary>
		/// <returns>
		/// Partial user object with userid and username or
		/// Complete user object with user info and user's conversations</returns>
		[HttpGet("{username}")]
		public async Task<IActionResult> GetUser([FromHeader] string authUsername, [FromHeader] string authPasswordHash, [FromRoute] string username)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var foundAuthUser = await _context.Users
				.SingleOrDefaultAsync(u => u.Username == authUsername && u.PasswordHash == authPasswordHash);

			if (foundAuthUser == null)
			{
				return StatusCode(403);
			}
			
			object foundUser;

			if (authUsername == username)
			{
				foundUser = await _context.Users
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
								.Where(uccuc => uccuc.User.Username != username)
								.Select(uccuc => new
								{
									User = new
									{
										uccuc.User.Username
									},
									uccuc.isAdmin
								})
						},
						uc.isAdmin
					})
				}).SingleOrDefaultAsync(u => u.Username == username);
			}
			else
			{
				foundUser = await _context.Users
				.Select(u => new
				{
					UserId = u.UserId,
					Username = u.Username,
				}).SingleOrDefaultAsync(u => u.Username == username);
			}

			if (foundUser == null)
			{
				return NotFound();
			}

			return Ok(foundUser);
		}
	}
}