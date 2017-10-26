﻿using Indigo.Core.Models;
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
        IIndigoApi Api = RestService.For<IIndigoApi>("http://192.168.0.2");

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
                return await Api.GetPageAsync(username);
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
            //Sends page to server
            return await Api.PostPageAsync(currentPage);

        }

        /// <summary>
        /// Tells server to update page in database
        /// </summary>
        /// <param name="currentPage">new version of page</param>
        public async Task PutPageAsync(Page currentPage)
        {
            await Api.PutPageAsync(currentPage.Name, currentPage);
        }
    }
}