using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Indigo.Core.Models;
using Indigo.Server.Context;

namespace Indigo.Server.Controllers
{
	[Produces("application/json")]
	[Route("api/UsersApi")]
	public class UsersApiController : Controller
	{
		private readonly IndigoContext _context;

		public UsersApiController(IndigoContext context)
		{
			_context = context;
		}

		// GET: api/UsersApi
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

			var foundUser = await _context.Users
				.Select(u => new
				{
					u.UserId,
					u.Username,
					u.PasswordHash,
					u.Email,
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
				return StatusCode(403);
			}

			return Accepted(foundUser);
		}

		// GET: api/UsersApi/username
		/// <summary>
		/// Requires user auth
		/// checks if the username matches a user and if it does return that users id
		/// </summary>
		/// <param name="publicusername">username to check for</param>
		/// <returns>Complete user object with user info, user's userconversations and the conversations for those</returns>
		[HttpGet]
		public async Task<IActionResult> GetPublicUser([FromHeader] string Username, [FromHeader] string PasswordHash, [FromRoute] string PublicUsername)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var foundAuthUser = await _context.Users
				.SingleOrDefaultAsync(u => u.Username == Username && u.PasswordHash == PasswordHash);

			if (foundAuthUser == null)
			{
				return StatusCode(403);
			}


			var foundUser = await _context.Users
				.Select(u => new
				{
					u.UserId,
					u.Username
				})
				.SingleOrDefaultAsync(u => u.Username == PublicUsername);

			if (foundUser == null)
			{
				return NotFound();
			}

			return Accepted(foundUser);
		}

		// PUT: api/UsersApi/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != user.UserId)
			{
				return BadRequest();
			}

			_context.Entry(user).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/UsersApi
		[HttpPost]
		public async Task<IActionResult> PostUser([FromBody] User user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (UserExists(user.Username))
			{
				return BadRequest();
			}

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUser", new { id = user.UserId }, user);
		}

		// DELETE: api/UsersApi/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await _context.Users.SingleOrDefaultAsync(m => m.UserId == id);
			if (user == null)
			{
				return NotFound();
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return Ok(user);
		}

		//TODO handle group chats
		//if removing user when users > 2 then just remove user
		//if removing user when users = 2 the do nothing and make deleter remove group entirely
		//TODO handle messages
		//TODO move to conversation api controller
		/// <summary>
		/// Requires user auth
		/// Takes a user id and a conversation id from the route and looks for that conversation and if
		/// the user id matches a user within that conversation and is auth user or auth user is admin within group
		/// then the user is removed from the group
		/// </summary>
		/// <param name="Username"></param>
		/// <param name="PasswordHash"></param>
		/// <param name="userid"></param>
		/// <param name="conversationid"></param>
		/// <returns></returns>
		[HttpDelete("{userid}/{conversationid}")]
		public async Task<IActionResult> DeleteUserCollection([FromHeader] string Username, [FromHeader] string PasswordHash,
			[FromRoute] int userid, [FromRoute] int conversationid)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var foundAuthUser = await _context.Users
				.SingleOrDefaultAsync(u => u.Username == Username && u.PasswordHash == PasswordHash);

			var foundUser = await _context.Users
				.SingleOrDefaultAsync(u => u.UserId == userid);

			if (foundAuthUser == null)
			{
				return StatusCode(403);
			}

			if (foundUser == null)
			{
				return NotFound();
			}

			var foundUserConversation = await _context.UserConversations
				.Include(uc => uc.Conversation)
					.ThenInclude(c => c.UserConversations)
				.SingleOrDefaultAsync(uc => uc.UserId == userid && uc.ConversationId == conversationid);

			if (foundUserConversation == null || foundUserConversation.Conversation.isGroupChat)
			{
				return NotFound();
			}

			if (foundAuthUser != foundUser)
			{
				var foundAuthUserConversation = await _context.UserConversations
					.Include(uc => uc.User)
					.SingleOrDefaultAsync(uc => uc.UserId == foundAuthUser.UserId && uc.ConversationId == conversationid);

				if (foundAuthUserConversation == null)
				{
					return NotFound();
				}

				if (!foundAuthUserConversation.isAdmin)
				{
					return StatusCode(403);
				}
			}

			_context.UserConversations.RemoveRange(foundUserConversation.Conversation.UserConversations);
			_context.Conversations.Remove(foundUserConversation.Conversation);
			await _context.SaveChangesAsync();

			return Ok();
		}

		private bool UserExists(int id)
		{
			return _context.Users.Any(e => e.UserId == id);
		}

		private bool UserExists(string Username)
		{
			return _context.Users.Any(e => e.Username == Username);
		}
	}
}