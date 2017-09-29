using Indigo.Core.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Indigo.Client.Core.Rest
{
    public class ServerAccess
    {
		IIndigoApi Api = Refit.RestService.For<IIndigoApi>("http://10.32.156.91/api");

		//TODO Handle error as error popup rather than crashing
		public async Task<User> CheckLoginAsync(string username, string passwordHash)
		{
			try
			{
				return await Api.CheckLoginAsync(username, passwordHash);
			}
			catch (ApiException e)
			{
				if (e.StatusCode == HttpStatusCode.NotFound) return null;
				throw;
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
	}
}
 