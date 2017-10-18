using Indigo.Client.Rest;
using Indigo.Core.Models;
using MvvmHelpers;
using System;
using System.Threading.Tasks;

namespace Indigo.Client.ViewModels
{
    public class PageViewModel : ObservableObject
    {
        public ServerAccess server = new ServerAccess();

        string _PageMessage;
        public string PageMessage
        {
            get => _PageMessage;
            set => SetProperty(ref _PageMessage, value);
        }

        string _LastEdited;
        public string LastEdited
        {
            get => _LastEdited;
            set => SetProperty(ref _LastEdited, value);
        }

        public async Task GetPageAsync(string pagename)
        {
            Page foundPage = await server.GetPageAsync(pagename);

            PageMessage = foundPage != null ? foundPage.Message : "";
            LastEdited = foundPage != null ?  foundPage.LastEdited.ToLocalTime().ToString("F") : "Never";
        }
    }
}