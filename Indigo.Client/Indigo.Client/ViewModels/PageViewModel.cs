using Indigo.Client.Rest;
using Indigo.Core.Models;
using Markdig;
using MvvmHelpers;
using System;
using System.Threading.Tasks;

namespace Indigo.Client.ViewModels
{
    public class PageViewModel : ObservableObject
    {
        public ServerAccess server = new ServerAccess();

        Page _page;
        public Page Page
        {
            get { return _page; }
            set { SetProperty(ref _page, value); }
        }

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

        Xamarin.Forms.HtmlWebViewSource _MarkdownView;
        public Xamarin.Forms.HtmlWebViewSource MarkdownView
        {
            get => _MarkdownView;
            set => SetProperty(ref _MarkdownView, value);
        }

        public PageViewModel()
        {
            Page = new Page
            {
                Name = "home"
            };
        }

        public async Task GetPageAsync(string pagename)
        {
            Page foundPage = await server.GetPageAsync(pagename);

            if (foundPage == null)
            {
                PageMessage = Page.Message = "";
                MarkdownView = new Xamarin.Forms.HtmlWebViewSource
                {
                    Html = ""
                };
                LastEdited = "Never";
                Page.PageId = 0;
            }
            else
            {
                PageMessage = Page.Message = foundPage.Message;
                MarkdownView = new Xamarin.Forms.HtmlWebViewSource
                {
                    Html = Markdown.ToHtml(PageMessage, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build())
                };
                LastEdited = foundPage.LastEdited.ToLocalTime().ToString("F");
                Page.PageId = foundPage.PageId;
            }
            Page.Name = pagename;
        }

        public async Task SavePageAsync()
        {
            Page currentPage = new Page
            {
                PageId = Page.PageId,
                Message = PageMessage,
                LastEdited = DateTime.UtcNow,
                Name = Page.Name
            };

            if (LastEdited == "Never")
            {
                await server.PostPageAsync(currentPage);
            }
            else
            {
                await server.PutPageAsync(currentPage);
            }

            LastEdited = currentPage.LastEdited.ToLocalTime().ToString("F");
        }
    }
}