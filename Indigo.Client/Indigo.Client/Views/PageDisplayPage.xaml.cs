using Indigo.Client.ViewModels;
using System.Threading.Tasks;
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

		public PageDisplayPage()
		{
			InitializeComponent();

            BindingContext = viewModel = new PageViewModel();
            saveButton = new ToolbarItem("Save Changes", "", async () =>
            {
                await viewModel.SavePageAsync();
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
        }
    }
}