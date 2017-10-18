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

		public PageDisplayPage()
		{
			InitializeComponent();

            BindingContext = viewModel = new PageViewModel();
        }

        private async void PageName_Changed(object sender, TextChangedEventArgs e)
        {
            string pageName = e.NewTextValue != "" ? e.NewTextValue : "home";
            await viewModel.GetPageAsync(pageName);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.GetPageAsync("home");
        }
    }
}