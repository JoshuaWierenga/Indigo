using Indigo.Core.Models;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Indigo.Client.Core.Rest
{
	public class ServerAccess
	{
		IIndigoApi Api = RestService.For<IIndigoApi>("http://192.168.42.242/api");

		public async Task<User> GetUserAsync(string username, string passwordHash)
		{
			try
			{
				return await Api.GetUserAsync(username, passwordHash);
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
				return await Api.CreateConversationAsync(authUser.Username, authUser.PasswordHash, conversation);
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

		public async Task<UserConversation> CreateUserConversationAsync(User authUser, User user, Conversation conversation)
		{
			try
			{
				return await Api.CreateUserConversationAsync(authUser.Username, authUser.PasswordHash, user.UserId, conversation.ConversationId);
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
				await Api.DeleteUserConversationAsync(user.Username, user.PasswordHash, user.UserId, conversation.ConversationId);
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