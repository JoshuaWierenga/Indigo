using Indigo.Core.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Indigo.Client.Core.Rest
{
    public interface IIndigoApi
    {
		[Get("/UsersApi")]
		Task<IEnumerable<User>> GetUsersAsync();

		[Get("/UsersApi/{id}")]
		Task<User> GetUserAsync(int id);

		[Post("/UsersApi")]
		Task<User> CreateUserAsync([Body] User user);

		[Put("/UsersApi/{id}")]
		Task EditUserAsync(int id, [Body] User user);
	}
}