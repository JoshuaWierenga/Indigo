using Indigo.Core.Models;
using Refit;
using System.Threading.Tasks;

namespace Indigo.Client.Core.Rest
{
    public interface IIndigoApi
    {
		[Get("/users/{username}")]
		Task<User> GetUserAsync([Header("authUsername")] string authUsername, [Header("authPasswordHash")] string authPasswordHash,
			string username);

		[Put("/conversations/{conversationid}")]
		Task PutConversationAsync([Header("authUsername")] string authUsername, [Header("authPasswordHash")] string authPasswordHash,
			int conversationid, [Body] Conversation conversation);

		[Post("/conversations")]
		Task<Conversation> PostConversationAsync([Header("authUsername")] string authUsername, [Header("authPasswordHash")] string authPasswordHash, 
			[Body] Conversation conversation);

		[Post("/conversations/{conversationid}/users")]
		Task<UserConversation> PostUserConversationAsync([Header("authUsername")] string authUsername, [Header("authPasswordHash")] string authPasswordHash,
			int conversationid, [Body] UserConversation userConversation);

		[Delete("/conversations/{conversationid}/users/{userid}")]
		Task DeleteUserConversationAsync([Header("authUsername")] string authUsername, [Header("authPasswordHash")] string authPasswordHash,
			int conversationid, int userid);
	}
}