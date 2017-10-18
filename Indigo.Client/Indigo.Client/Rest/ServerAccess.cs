using Indigo.Core.Models;
using Refit;
using System.Net;
using System.Threading.Tasks;

namespace Indigo.Client.Rest
{
    public class ServerAccess
    {
        IIndigoApi Api = RestService.For<IIndigoApi>("http://192.168.0.2");

        public async Task<Page> GetPageAsync(string username)
        {
            try
            {
                return await Api.GetPageAsync(username);
            }
            catch (ApiException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }
    }
}