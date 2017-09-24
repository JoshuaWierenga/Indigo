using Indigo.Client.Core.ViewModels;
using Indigo.Core.Models;
using System;
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