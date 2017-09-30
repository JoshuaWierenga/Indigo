using Indigo.Client.Core.ViewModels;
using Indigo.Core.Models;
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
			User newUser = await viewModel.Server.CheckLoginAsync(viewModel.Username, viewModel.PasswordHash);

			if(newUser != null)
			{
				await Navigation.PushAsync(new ConversationsPage(newUser));
			}
			
		}
	}
}