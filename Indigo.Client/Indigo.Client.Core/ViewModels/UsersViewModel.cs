using Indigo.Client.Core.Views;
using Indigo.Core.Models;
using MvvmHelpers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace Indigo.Client.Core.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
		public ObservableRangeCollection<User> Users { get; set; }
		
		public UsersViewModel()
		{
			Users = new ObservableRangeCollection<User>();

			MessagingCenter.Subscribe<ModifyUserPage, User>(this, "AddUser", async (obj, user) =>
			{
				await Api.CreateUserAsync(user as User);
				await UpdateAllUsersAsync();
			});

			MessagingCenter.Subscribe<ModifyUserPage, User>(this, "EditUser", async (obj, user) =>
			{
				User _user = user as User;
				await Api.EditUserAsync(_user.UserId, _user);
				await UpdateAllUsersAsync();
			});
		}

		public async Task UpdateAllUsersAsync()
		{
			try
			{
				Users.ReplaceRange(await Api.GetUsersAsync());
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}