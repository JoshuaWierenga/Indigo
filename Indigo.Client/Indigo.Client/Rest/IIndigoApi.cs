using Indigo.Core.Models;
using Refit;
using System.Threading.Tasks;

namespace Indigo.Client.Rest
{
    /// <summary>
    /// Interface for how to handle server connections
    /// </summary>
    public interface IIndigoApi
    {
        /// <summary>
        /// Not full code, used by Rest to generate connection code
        /// requests page object from server
        /// </summary>
        /// <param name="pagename">name of page to get object of</param>
        /// <returns>Full page object</returns>
        [Get("/api/{pagename}")]
        Task<Page> GetPageAsync(string pagename);

        /// <summary>
        /// Not full code, used by Rest to generate connection code
        /// Tells server to create page in database
        /// </summary>
        /// <param name="currentPage">page to send to server</param>
        /// <returns>Full page object</returns>
        [Post("/api")]
        Task<Page> PostPageAsync(Page currentPage);

        /// <summary>
        /// Not full code, used by Rest to generate connection code
        /// Tells server to update page in database
        /// </summary>
        /// <param name="pagename">name of page to update</param>
        /// <param name="currentPage">new version of page</param>
        [Put("/api/{pagename}")]
        Task PutPageAsync(string pagename, Page currentPage);
    }
}