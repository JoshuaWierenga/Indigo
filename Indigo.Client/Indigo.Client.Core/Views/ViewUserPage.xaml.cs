using Indigo.Client.Core.ViewModels;
using Indigo.Core.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewUserPage : ContentPage
	{
		ViewUserViewModel viewModel;

		public ViewUserPage (ViewUserViewModel existingViewModel = null)
		{
			InitializeComponent ();

			BindingContext = viewModel = existingViewModel ?? new ViewUserViewModel();
		}

		async void OnEditButtonPressed(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ModifyUserPage(new ModifyUserViewModel(viewModel.user.UserId)));
		}

		async void OnDeleteButtonPressed(object sender, EventArgs e)
		{
			bool deleteUser = await DisplayAlert("Warning", "Are you sure you wish to delete this user", "yes", "no");
			if (deleteUser)
			{
				await viewModel.Server.DeleteUserAsync(viewModel.user);
				await Navigation.PopToRootAsync();
			}
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await viewModel.GetExistingUser();
		}

		protected override bool OnBackButtonPressed()
		{
			Navigation.PopToRootAsync();
			return true;
		}
	}
}