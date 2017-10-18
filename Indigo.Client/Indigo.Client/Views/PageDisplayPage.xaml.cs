using Indigo.Client.ViewModels;

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

            NameEntry.Unfocus();
        }
	}
}