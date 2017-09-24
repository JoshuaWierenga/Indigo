using Indigo.Client.Core.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewUserPage : ContentPage
	{
		NewUserViewModel viewModel;

		public NewUserPage ()
		{
			InitializeComponent ();

			BindingContext = viewModel = new NewUserViewModel();
		}

		async void Create_Clicked(object sender, EventArgs e)
		{
			if (!((ValidateInput("Username") & ValidateInput("PasswordHash") & ValidateInput("Email")))) return;

			MessagingCenter.Send(this, "AddUser", viewModel.user);
			await Navigation.PopToRootAsync();
		}

		bool ValidateInput(string input)
		{
			string inputValue = viewModel.user.GetType().GetProperty(input).GetValue(viewModel.user) as string;
			bool valid = !string.IsNullOrWhiteSpace(inputValue);

			viewModel.GetType().GetProperty(input + "Text").SetValue(viewModel, !valid ? "Can not be empty" : "");
			return valid;
		}
	}
}