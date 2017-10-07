using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Indigo.Core.Models;
using Indigo.Server.Context;

namespace Indigo.Server.Controllers
{
	[Produces("application/json")]
	[Route("api/ConversationsApi")]
	public class ConversationsApiController : Controller
	{
		private readonly IndigoContext _context;

		public ConversationsApiController(IndigoContext context)
		{
			_context = context;
		}

		// GET: api/ConversationsApi/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetConversation([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var conversation = await _context.Conversations.SingleOrDefaultAsync(m => m.ConversationId == id);

			if (conversation == null)
			{
				return NotFound();
			}

			return Ok(conversation);
		}

		// PUT: api/ConversationsApi/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutConversation([FromRoute] int id, [FromBody] Conversation conversation)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != conversation.ConversationId)
			{
				return BadRequest();
			}

			_context.Entry(conversation).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ConversationExists(id))
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

		// POST: api/ConversationsApi
		/// <summary>
		/// Requires user auth
		/// Takes a partial conversation object containing a name and a chat type
		/// and creates the conversation in the database and returns the full conversation object
		/// </summary>
		/// <param name="conversation">Partial conversation object containing a name and a chat type</param>
		/// <returns>Full conversation object including the database id</returns>
		[HttpPost]
		public async Task<IActionResult> PostConversation([FromHeader] string Username, [FromHeader] string PasswordHash, [FromBody] Conversation conversation)
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

			_context.Conversations.Add(conversation);
			await _context.SaveChangesAsync();


			return Ok(conversation);
		}

		// DELETE: api/ConversationsApi/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteConversation([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var conversation = await _context.Conversations.SingleOrDefaultAsync(m => m.ConversationId == id);
			if (conversation == null)
			{
				return NotFound();
			}

			_context.Conversations.Remove(conversation);
			await _context.SaveChangesAsync();

			return Ok(conversation);
		}

		// POST: api/ConversationsApi/5/5
		/// <summary>
		/// Requires user auth
		/// Takes a conversation id and a Userconversation object containing a user id, conversation id and if or not the user should be an admin
		/// if both exist then creates a userconversation linking them and returns it
		/// </summary>
		/// <param name="userid">user to link</param>
		/// <param name="conversationid">conversation to link</param>
		/// <returns>Userconversation linking the user to the conversation</returns>
		[HttpPost("{conversationid}")]
		public async Task<IActionResult> PostUserConversationAsync([FromHeader] string Username, [FromHeader] string PasswordHash,
			[FromRoute] int conversationid, [FromBody] UserConversation userConversation)
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
				.SingleOrDefaultAsync(u => u.UserId == userConversation.UserId);

			var foundConversation = await _context.Conversations
				.Include(c => c.UserConversations)
				.SingleOrDefaultAsync(c => c.ConversationId == userConversation.ConversationId);

			if (foundUser == null || foundConversation == null || conversationid != userConversation.ConversationId)
			{
				return NotFound();
			}

			if (foundConversation.UserConversations.SingleOrDefault(uc => uc.UserId == userConversation.UserId) != null)
			{
				return NotFound();
			}

			userConversation.User = foundUser;
			userConversation.Conversation = foundConversation;

			_context.UserConversations.Add(userConversation);
			await _context.SaveChangesAsync();

			return Ok(userConversation);
		}

		private bool ConversationExists(int id)
        {
            return _context.Conversations.Any(e => e.ConversationId == id);
        }
    }
}