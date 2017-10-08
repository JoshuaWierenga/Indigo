using Indigo.Core.Models;
using Refit;
using System.Threading.Tasks;

namespace Indigo.Client.Core.Rest
{
    public interface IIndigoApi
    {
		[Get("/UsersApi")]
		Task<User> GetUserAsync([Header("Username")] string Username, [Header("PasswordHash")] string PasswordHash);

		[Put("/ConversationsApi")]
		Task<Conversation> CreateConversationAsync([Header("Username")] string Username, [Header("PasswordHash")] string PasswordHash, Conversation conversation);

		[Post("/ConversationsApi/{conversationid}/{userid}")]
		Task<UserConversation> CreateUserConversationAsync([Header("Username")] string Username, [Header("PasswordHash")] string PasswordHash, int userid, int conversationid);

		[Delete("/UsersApi/{userid}/{conversationid}")]
		Task DeleteUserConversationAsync([Header("Username")] string Username, [Header("PasswordHash")] string PasswordHash, int userid, int conversationid);
	}
}