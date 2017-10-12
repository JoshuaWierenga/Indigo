using Indigo.Core.Models;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Indigo.Client.Core.Rest
{
	public class ServerAccess
	{
		IIndigoApi Api = RestService.For<IIndigoApi>("http://192.168.0.2/api");

		public async Task<User> GetUserAsync(User authUser, string username = null)
		{
			try
			{
				if (username == null)
				{
					username = authUser.Username;
				}

				return await Api.GetUserAsync(authUser.Username, authUser.PasswordHash, username);
			}
			catch (ApiException e)
			{
				if (e.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					//TODO Check for this and display to user
					MessagingCenter.Send(this, "Forbidden");
					return null;
				}
				throw;
			}
			catch (HttpRequestException)
			{
				//TODO Check for this and display to user
				MessagingCenter.Send(this, "HttpRequestException");
				return null;
			}
		}

		public async Task<Conversation> CreateConversationAsync(User authUser, Conversation conversation)
		{
			try
			{
				return await Api.PostConversationAsync(authUser.Username, authUser.PasswordHash, conversation);
			}
			catch (ApiException e)
			{
				if (e.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					//TODO Check for this and display to user
					MessagingCenter.Send(this, "Forbidden");
					return null;
				}
				throw;
			}
			catch (HttpRequestException)
			{
				//TODO Check for this and display to user
				MessagingCenter.Send(this, "HttpRequestException");
				return null;
			}
		}

		public async Task<UserConversation> CreateUserConversationAsync(User authUser, Conversation conversation, User newUser, bool admin)
		{
			try
			{
				UserConversation newUserConversation = new UserConversation
				{
					ConversationId = conversation.ConversationId,
					UserId = newUser.UserId,
					isAdmin = admin
				};

				return await Api.PostUserConversationAsync(authUser.Username, authUser.PasswordHash, conversation.ConversationId, newUserConversation);
			}
			catch (ApiException e)
			{
				if (e.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					//TODO Check for this and display to user
					MessagingCenter.Send(this, "Forbidden");
					return null;
				}
				throw;
			}
			catch (HttpRequestException)
			{
				//TODO Check for this and display to user
				MessagingCenter.Send(this, "HttpRequestException");
				return null;
			}
		}

		public async Task DeleteUserConversationAsync(User user, Conversation conversation)
		{
			try
			{
				await Api.DeleteUserConversationAsync(user.Username, user.PasswordHash, conversation.ConversationId, user.UserId);
			}
			catch (ApiException e)
			{
				if (e.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					//TODO Check for this and display to user
					MessagingCenter.Send(this, "Forbidden");
				}
				else throw;
			}
			catch (HttpRequestException)
			{
				//TODO Check for this and display to user
				MessagingCenter.Send(this, "HttpRequestException");
			}
		}
	}
}