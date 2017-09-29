using Indigo.Core.Models;
using Refit;
using System.Threading.Tasks;

namespace Indigo.Client.Core.Rest
{
    public interface IIndigoApi
    {

		[Get("/UsersApi")]
		Task<User> CheckLoginAsync([Header("Username")] string Username, [Header("PasswordHash")] string PasswordHash);

		[Post("/UsersApi")]
		Task<User> CreateUserAsync([Body] User user);

		[Put("/UsersApi/{id}")]
		Task EditUserAsync(int id, [Body] User user);

		[Delete("/UsersApi/{id}")]
		Task DeleteUserAsync(int id);
	}
}