using Indigo.Client.Core.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyUserPage : ContentPage
	{
		ModifyUserViewModel viewModel;

		public ModifyUserPage(ModifyUserViewModel existingViewModel = null)
		{
			InitializeComponent();

			BindingContext = viewModel = existingViewModel ?? new ModifyUserViewModel();
		}

		async void Finish_Clicked(object sender, EventArgs e)
		{
			if (!((viewModel.ValidateInput("Username") 
				 & viewModel.ValidateInput("PasswordHash")
				 & viewModel.ValidateInput("Email")))) return;

			MessagingCenter.Send(this, viewModel.newUser ? "AddUser" : "EditUser", viewModel.user);

			await Navigation.PopToRootAsync();
		}
	}
}