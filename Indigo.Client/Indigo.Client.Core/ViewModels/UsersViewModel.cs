using Indigo.Client.Core.Views;
using Indigo.Core.Models;
using MvvmHelpers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Indigo.Client.Core.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
		public ObservableRangeCollection<User> Users { get; set; }
		
		public UsersViewModel()
		{
			Users = new ObservableRangeCollection<User>();

			MessagingCenter.Subscribe<NewUserPage, User>(this, "AddUser", async (obj, user) =>
			{
				await Api.CreateUserAsync(user as User);
				await UpdateAllUsersAsync();
			});
		}

		public async Task UpdateAllUsersAsync()
		{
			try
			{
				Users.Clear();
				Users.AddRange(await Api.GetUsersAsync());
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}