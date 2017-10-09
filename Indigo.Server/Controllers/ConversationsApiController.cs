using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Indigo.Core.Models;
using Indigo.Server.Context;
using System.Collections.Generic;

namespace Indigo.Server.Controllers
{
	[Produces("application/json")]
	[Route("api/conversations")]
	public class ConversationsApiController : Controller
	{
		private readonly IndigoContext _context;

		public ConversationsApiController(IndigoContext context)
		{
			_context = context;
		}

		// POST: api/conversations
		/// <summary>
		/// Requires user auth
		/// Takes a partial conversation object containing a name and a chat type
		/// and creates the conversation in the database with the auth user as admin and returns the full conversation object
		/// </summary>
		/// <param name="conversation">Partial conversation object containing a name and a chat type</param>
		/// <returns>Full conversation object including the database id</returns>
		[HttpPost]
		public async Task<IActionResult> PostConversation([FromHeader] string authUsername, [FromHeader] string authPasswordHash, [FromBody] Conversation conversation)
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

			if (conversation.ConversationName == null)
			{
				return BadRequest();
			}

			conversation.UserConversations = new List<UserConversation>
			{
				new UserConversation
				{
					UserId = foundAuthUser.UserId,
					isAdmin = true
				}
			};

			_context.Conversations.Add(conversation);
			await _context.SaveChangesAsync();

			return Ok(new
			{
				conversation.ConversationId,
				conversation.ConversationName,
				conversation.isGroupChat,
				UserConversations = conversation.UserConversations.Select(uc => new
				{
					User = new
					{
						uc.User.UserId,
						uc.User.Username
					},
					uc.isAdmin
				})
			});

		}

		// POST: api/conversations/5/users
		/// <summary>
		/// Requires user auth
		/// Takes a conversation id from the route and a userconversation object containing
		/// a user id, a conversation id and user permission level
		/// and creates the userconversation in the database and returns it
		/// </summary>
		/// <param name="userid">id of user</param>
		/// <param name="conversationid">id of conversation</param>
		/// <returns>Userconversation object containing conversation id and name, user id and name and user permission level</returns>
		[HttpPost("{conversationid}/users")]
		public async Task<IActionResult> PostUserConversationAsync([FromHeader] string authUsername, [FromHeader] string authPasswordHash,
			[FromRoute] int conversationid, [FromBody] UserConversation userConversation)
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

			var foundUser = await _context.Users
				.SingleOrDefaultAsync(u => u.UserId == userConversation.UserId);

			var foundConversation = await _context.Conversations
				.Include(c => c.UserConversations)
				.SingleOrDefaultAsync(c => c.ConversationId == userConversation.ConversationId);

			if (foundUser == null || foundConversation == null || conversationid != userConversation.ConversationId
				|| foundConversation.UserConversations.SingleOrDefault(uc => uc.UserId == userConversation.UserId) != null)
			{
				return NotFound();
			}

			_context.UserConversations.Add(userConversation);
			await _context.SaveChangesAsync();

			return Ok(new
			{
				Conversation = new
				{
					userConversation.Conversation.ConversationId,
					userConversation.Conversation.ConversationName
				},
				User = new
				{
					userConversation.User.UserId,
					userConversation.User.Username
				},
				userConversation.isAdmin
			});
		}

		// Post: api/conversations/5/users/5
		//TODO handle group chats rather than just deleting private chats
		//if removing user when users > 2 then just remove user
		//if removing user when users = 2 the do nothing and make deleter remove group entirely
		//TODO handle messages
		/// <summary>
		/// Requires user auth
		/// Takes a user id and a conversation id from the route and attempts to remove the conversation
		/// </summary>
		/// <param name="userid">id of user</param>
		/// <param name="conversationid">id of conversation</param>
		/// <returns></returns>
		[HttpDelete("{conversationid}/users/{userid}")]
		public async Task<IActionResult> DeleteUserCollection([FromHeader] string authUsername, [FromHeader] string authPasswordHash, 
			[FromRoute] int conversationid, [FromRoute] int userid)
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

			var foundUser = await _context.Users
				.SingleOrDefaultAsync(u => u.UserId == userid);

			var foundUserConversation = await _context.UserConversations
				.Include(uc => uc.Conversation)
					.ThenInclude(c => c.UserConversations)
				.SingleOrDefaultAsync(uc => uc.UserId == userid && uc.ConversationId == conversationid);

			if (foundUser == null || foundUserConversation == null || foundUserConversation.Conversation.isGroupChat)
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

			_context.Conversations.Remove(foundUserConversation.Conversation);
			await _context.SaveChangesAsync();

			return NoContent();
		}
    }
}