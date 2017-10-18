using Indigo.Core.Models;
using Refit;
using System.Threading.Tasks;

namespace Indigo.Client.Rest
{
    public interface IIndigoApi
    {
        [Get("/api/{pagename}")]
        Task<Page> GetPageAsync(string pagename);

        [Post("/api")]
        Task<Page> PostPageAsync(Page currentPage);

        [Put("/api/{pagename}")]
        Task PutPageAsync(string pagename, Page currentPage);
    }
}