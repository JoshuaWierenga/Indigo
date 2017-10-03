using Indigo.Core.Models;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Indigo.Client.Core.Rest
{
	public class ServerAccess
	{
		IIndigoApi Api = RestService.For<IIndigoApi>("http://192.168.0.2/api");

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

		//TODO Handle error as error popup rather than crashing
		public async Task<User> CreateUserAsync(User user)
		{
			try
			{
				return await Api.CreateUserAsync(user);
			}
			catch (Exception)
			{
				throw;
			}
		}

		//TODO Handle error as error popup rather than crashing
		public async Task EditUserAsync(User user)
		{
			try
			{
				await Api.EditUserAsync(user.UserId, user);
			}
			catch (Exception)
			{
				throw;
			}
		}

		//TODO Handle error as error popup rather than crashing
		public async Task DeleteUserAsync(User user)
		{
			try
			{
				await Api.DeleteUserAsync(user.UserId);
			}
			catch (Exception)
			{

				throw;
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