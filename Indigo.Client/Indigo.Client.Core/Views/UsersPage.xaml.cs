using Indigo.Client.Core.ViewModels;
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

		public void AddUser_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new NewUserPage());
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if(viewModel.Users.Count == 0)
			{
				await viewModel.UpdateAllUsersAsync();
			}
		}
	}
}