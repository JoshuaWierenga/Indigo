using Indigo.Core.Models;
using Refit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Indigo.Client.Rest
{
    /// <summary>
    /// Handles server connections
    /// </summary>
    public class ServerAccess
    {
        /// <summary>
        /// Uses rest to create methods for IIndigoApi
        /// </summary>
        private readonly IIndigoApi _api = RestService.For<IIndigoApi>("http://localhost:5000");

        /// <summary>
        /// Requests page object from server
        /// </summary>
        /// <param name="username">name of page to get object of</param>
        /// <returns>Full page object</returns>
        public async Task<Page> GetPageAsync(string username)
        {
            //Attempts to send request to server
            try
            {
                //Sends request for page to server
                return await _api.GetPageAsync(username);
            }
            catch (ApiException e)
            {
                //check if error cpde is http status 404
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
            catch (HttpRequestException)
            {
                //sends message to the viewmodel that there was a httprequestexception
                Xamarin.Forms.MessagingCenter.Send(this, "httprequestexception");
                return null;
            }
        }

        /// <summary>
        /// Tells server to create page in database
        /// </summary>
        /// <param name="currentPage">page to send to server</param>
        /// <returns>Full page object</returns>
        public async Task<Page> PostPageAsync(Page currentPage)
        {
            //Attempts to send page to server
            try
            {
                //Sends page to server
                return await _api.PostPageAsync(currentPage);
            }
            catch (HttpRequestException)
            {
                //sends message to the viewmodel that there was a httprequestexception
                Xamarin.Forms.MessagingCenter.Send(this, "httprequestexception");
                return null;
            }
        }

        /// <summary>
        /// Tells server to update page in database
        /// </summary>
        /// <param name="currentPage">new version of page</param>
        public async Task PutPageAsync(Page currentPage)
        {
            //Attempts to send page to server
            try
            {
                //Sends page to server
                await _api.PutPageAsync(currentPage.Name, currentPage);
            }
            catch (HttpRequestException)
            {
                //sends message to the viewmodel that there was a httprequestexception
                Xamarin.Forms.MessagingCenter.Send(this, "httprequestexception");
            }          
        }
    }
}