using Indigo.Client.ViewModels;
using ViewMarkdown.Forms.Plugin.Abstractions;
using CommonMark;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageDisplayPage : ContentPage
	{
        PageViewModel viewModel;
        ToolbarItem saveButton;
        bool saveDisplayed;

        WebView webView;

		public PageDisplayPage()
		{
			InitializeComponent();

            BindingContext = viewModel = new PageViewModel();

            MarkdownView markdownView = new MarkdownView
            {
                Markdown = "## h2",
                VerticalOptions = LayoutOptions.FillAndExpand
            };


            webView = new WebView
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            };


            pagedisplayer.Children.Add(webView);

            saveButton = new ToolbarItem("Save Changes", "ic_save.png", async () =>
            {
                await viewModel.SavePageAsync();
                PageMessage_Changed(this, new TextChangedEventArgs("", viewModel.Page.Message));
            });
            saveDisplayed = false;
        }

        async void PageName_Changed(object sender, TextChangedEventArgs e)
        {
            string pageName = e.NewTextValue != "" ? e.NewTextValue : "home";
            await viewModel.GetPageAsync(pageName);
        }

        void PageMessage_Changed(object sender, TextChangedEventArgs e)
        {
            if (viewModel.Page.Message != e.NewTextValue && !saveDisplayed)
            {
                saveDisplayed = true;
                ToolbarItems.Add(saveButton);
            }
            else if (viewModel.Page.Message == e.NewTextValue && saveDisplayed)
            {
                ToolbarItems.Remove(saveButton);
                saveDisplayed = false;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.GetPageAsync("home");
            webView.Source = new HtmlWebViewSource
            {
                Html = CommonMarkConverter.Convert(viewModel.PageMessage)
            };
        }
    }
}