using Indigo.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Markdig;
using System;
using System.Threading.Tasks;

namespace Indigo.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDisplayPage : ContentPage
    {
        PageViewModel viewModel;
        ToolbarItem editSaveButton;
        WebView webView;

        public PageDisplayPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new PageViewModel();

            webView = new WebView
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            editSaveButton = new ToolbarItem("Edit Page", "ic_edit.png", async () => await EditSave_Clicked());

            pageView.Children.Add(webView);
            ToolbarItems.Add(editSaveButton);
        }

        async void PageName_Changed(object sender, TextChangedEventArgs e)
        {
            string pageName = e.NewTextValue != "" ? e.NewTextValue : "home";
            await viewModel.GetPageAsync(pageName);
            UpdateMarkdownView();
        }

        async Task EditSave_Clicked()
        {
            if (editSaveButton.Text == "Edit Page")
            {
                editSaveButton.Text = "Save Changes";
                editSaveButton.Icon = "ic_save.png";
                webView.IsVisible = false;
                pageEditor.IsVisible = true;
            }
            else
            {
                if (viewModel.PageMessage != viewModel.Page.Message)
                {
                    await viewModel.SavePageAsync();
                    UpdateMarkdownView();
                }
                
                editSaveButton.Text = "Edit Page";
                editSaveButton.Icon = "ic_edit.png";
                webView.IsVisible = true;
                pageEditor.IsVisible = false;                
            }
        }

        void UpdateMarkdownView()
        {
            webView.Source = new HtmlWebViewSource
            {
                Html = Markdown.ToHtml(viewModel.PageMessage, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build())
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.GetPageAsync("home");
            UpdateMarkdownView();
        }
    }
}