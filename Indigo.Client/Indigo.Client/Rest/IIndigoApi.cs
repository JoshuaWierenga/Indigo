using Indigo.Core.Models;
using Refit;
using System.Threading.Tasks;

namespace Indigo.Client.Rest
{
    public interface IIndigoApi
    {
        [Get("/api/{pagename}")]
        Task<Page> GetPageAsync(string pagename);
    }
}