using Indigo.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Indigo.Client.Core.Rest
{
    public class ServerAccess
    {
		IIndigoApi Api = Refit.RestService.For<IIndigoApi>("http://192.168.0.2/api");

		//TODO Handle error as error popup rather than crashing
		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			try
			{
				return await Api.GetUsersAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		//TODO Handle error as error popup rather than crashing
		public async Task<User> GetUserAsync(int id)
		{
			try
			{
				return await Api.GetUserAsync(id);
			}
			catch (Exception)
			{
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
	}
}
 