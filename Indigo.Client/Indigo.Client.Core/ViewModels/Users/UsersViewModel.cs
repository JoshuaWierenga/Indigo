using Indigo.Client.Core.Views;
using Indigo.Core.Models;
using MvvmHelpers;
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

			MessagingCenter.Subscribe<ModifyUserPage, User>(this, "AddUser", async (obj, user) =>
			{
				await Server.CreateUserAsync(user as User);
				await UpdateUsersAsync();
			});

			MessagingCenter.Subscribe<ModifyUserPage, User>(this, "EditUser", async (obj, user) =>
			{
				await Server.EditUserAsync(user as User);
				await UpdateUsersAsync();
			});
		}

		public async Task UpdateUsersAsync()
		{
			//Users.ReplaceRange(await Server.GetAllUsersAsync());
		}
	}
}