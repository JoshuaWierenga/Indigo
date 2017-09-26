using Indigo.Client.Core.ViewModels;
using Indigo.Core.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UsersPage : ContentPage
	{
		UsersViewModel viewModel;

		public UsersPage ()
		{
			InitializeComponent();

			BindingContext = viewModel = new UsersViewModel();
		}

		async void AddUser_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ModifyUserPage());
		}

		async void User_Clicked(object sender, SelectedItemChangedEventArgs e)
		{
			User user = e.SelectedItem as User;
			if (user == null) return;

			await Navigation.PushAsync(new ViewUserPage(new ViewUserViewModel(user.UserId)));
			UsersListView.SelectedItem = null;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			Device.StartTimer(TimeSpan.FromSeconds(10), () =>
			{
				viewModel.UpdateUsersAsync();
				return true;
			});
		}
	}
}