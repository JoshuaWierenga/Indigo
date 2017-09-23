using Indigo.Core.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Indigo.Client.Core.Rest
{
    public interface IIndigoApi
    {
		[Get("/UsersApi")]
		Task<List<User>> GetUsers();
    }
}