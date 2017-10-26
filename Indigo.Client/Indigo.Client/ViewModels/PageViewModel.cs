using Indigo.Client.Rest;
using Indigo.Core.Models;
using Markdig;
using MvvmHelpers;
using System;
using System.Threading.Tasks;

namespace Indigo.Client.ViewModels
{
    /// <summary>
    /// Contains the functions and variables used by the view
    /// </summary>
    public class PageViewModel : ObservableObject
    {
        public ServerAccess server = new ServerAccess();

        Page _page;
        /// <summary>
        /// Provides access to the page while alerting anything that is binded to the page whenever the page is updated    
        /// </summary>
        public Page Page
        {
            get { return _page; }
            set { SetProperty(ref _page, value); }
        }

        string _PageMessage;
        /// <summary>
        /// Provides access to the message while alerting anything that is binded to the page whenever the page is updated    
        /// </summary>
        public string PageMessage
        {
            get => _PageMessage;
            set => SetProperty(ref _PageMessage, value);
        }

        string _LastEdited;
        /// <summary>
        /// Provides access to the last edit time while alerting anything that is binded to the page whenever the page is updated    
        /// </summary>
        public string LastEdited
        {
            get => _LastEdited;
            set => SetProperty(ref _LastEdited, value);
        }

        Xamarin.Forms.HtmlWebViewSource _MarkdownView;
        /// <summary>
        /// Provides access to the html code for the page while alerting anything that is binded to the page whenever the page is updated
        /// </summary>
        public Xamarin.Forms.HtmlWebViewSource MarkdownView
        {
            get => _MarkdownView;
            set => SetProperty(ref _MarkdownView, value);
        }

        bool _Loading;
        /// <summary>
        /// Provides access to the loading state while alerting anything that is binded to the page whenever the page is updated    
        /// </summary>
        public bool Loading
        {
            get => _Loading;
            set => SetProperty(ref _Loading, value);
        }

        /// <summary>
        /// Sets page to home page
        /// </summary>
        public PageViewModel()
        {
            //creates new page set to home page
            Page = new Page
            {
                Name = "home"
            };
        }

        /// <summary>
        /// Gets page from the server and then saves it so the view updates
        /// </summary>
        /// <param name="pagename">name of page to retrieve</param>
        public async Task GetPageAsync(string pagename)
        {
            Loading = true;

            //subscribe to connection issues
            bool connectionError = false;
            Xamarin.Forms.MessagingCenter.Subscribe<ServerAccess>(this, "httprequestexception", (sender) =>
            {
                connectionError = true;
                //sends message to the view that there was a httprequestexception
                Xamarin.Forms.MessagingCenter.Send(this, "connection error");
            });

            //Gets page from server
            Page foundPage = await server.GetPageAsync(pagename);

            //keeps retrying until page can be retrieved from the server
            while (connectionError)
            {
                connectionError = false;
                //Gets page from server
                foundPage = await server.GetPageAsync(pagename);
            }
            //unsubscribe to connection issues
            Xamarin.Forms.MessagingCenter.Unsubscribe<ServerAccess>(this, "httprequestexception");

            //check if page exists
            if (foundPage == null)
            {
                //clears page name in view
                PageMessage = Page.Message = "";
                //clears markdown view in view
                MarkdownView = new Xamarin.Forms.HtmlWebViewSource
                {
                    Html = ""
                };
                //shows last edit time as never in view
                LastEdited = "Never";
                //resets page id
                Page.PageId = 0;
            }
            else
            {
                //shows page message in view
                PageMessage = Page.Message = foundPage.Message;
                //shows page message as markdown in view
                MarkdownView = new Xamarin.Forms.HtmlWebViewSource
                {
                    //converts page message to html via markdown
                    Html = Markdown.ToHtml(PageMessage, new MarkdownPipelineBuilder().UseAdvancedExtensions().UseEmojiAndSmiley().Build())
                };
                //shows last update time in view
                LastEdited = foundPage.LastEdited.ToLocalTime().ToString("F");
                //sets page id so page can be saved later
                Page.PageId = foundPage.PageId;
            }
            Page.Name = pagename;
            Loading = false;
        }

        /// <summary>
        /// Saves page to the server
        /// </summary>
        public async Task SavePageAsync()
        {
            //Creates full page object from existing id, message, name and current time
            Page currentPage = new Page
            {
                PageId = Page.PageId,
                Message = PageMessage,
                LastEdited = DateTime.UtcNow,
                Name = Page.Name
            };

            //check if page is new
            if (LastEdited == "Never")
            {
                //create page in database
                await server.PostPageAsync(currentPage);
            }
            else
            {
                //update page in database
                await server.PutPageAsync(currentPage);
            }

            //updates last edit time
            LastEdited = currentPage.LastEdited.ToLocalTime().ToString("F");
        }
    }
}