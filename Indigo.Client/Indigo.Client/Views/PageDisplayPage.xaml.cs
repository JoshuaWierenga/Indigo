using Indigo.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Markdig;
using System.Threading.Tasks;

namespace Indigo.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDisplayPage : ContentPage
    {
        PageViewModel viewModel;
        ToolbarItem editSaveButton;

        public PageDisplayPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new PageViewModel();

            editSaveButton = new ToolbarItem("Edit Page", "ic_edit.png", async () => await EditSave_Clicked());

            ToolbarItems.Add(editSaveButton);
        }

        async void PageName_Changed(object sender, TextChangedEventArgs e)
        {
            string pageName = e.NewTextValue != "" ? e.NewTextValue : "home";
            await viewModel.GetPageAsync(pageName);
        }

        async Task EditSave_Clicked()
        {
            if (editSaveButton.Text == "Edit Page")
            {
                editSaveButton.Text = "Save Changes";
                editSaveButton.Icon = "ic_save.png";
                markdownViewer.IsVisible = false;
                pageEditor.IsVisible = true;
            }
            else
            {
                if (viewModel.PageMessage != viewModel.Page.Message)
                {
                    await viewModel.SavePageAsync();
                }
                
                editSaveButton.Text = "Edit Page";
                editSaveButton.Icon = "ic_edit.png";
                markdownViewer.IsVisible = true;
                pageEditor.IsVisible = false;                
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.GetPageAsync("home");
        }
    }
}