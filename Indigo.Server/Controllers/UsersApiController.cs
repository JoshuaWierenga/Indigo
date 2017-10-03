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
		/// Takes a partial user object and checks if password is correct
		/// and returns the full user object if it is
		/// </summary>
		/// <param name="user">Partial user object containing username and password</param>
		/// <returns></returns>
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

		[HttpDelete("{userid}/{conversationid}")]
		public async Task<IActionResult> DelteUserCollection([FromHeader] string Username, [FromHeader] string PasswordHash ,
			[FromRoute] int userid, [FromRoute] int conversationid)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var foundUser = await _context.Users
				.SingleOrDefaultAsync(u => u.UserId == userid && u.Username == Username && u.PasswordHash == PasswordHash);

			if (foundUser == null)
			{
				return StatusCode(403);
			}

			var foundUserConversation = await _context.UserConversations
				.SingleOrDefaultAsync(uc => uc.UserId == userid && uc.ConversationId == conversationid);

			if (foundUserConversation == null)
			{
				return NotFound();
			}

			_context.UserConversations.Remove(foundUserConversation);
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