using Indigo.Client.Core.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		LoginViewModel viewModel;

		public LoginPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new LoginViewModel();
		}

		async void Login_Clicked(object sender, EventArgs e)
		{
			
		}
	}
}