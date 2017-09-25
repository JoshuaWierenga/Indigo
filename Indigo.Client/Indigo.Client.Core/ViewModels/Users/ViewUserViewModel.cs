using Indigo.Core.Models;
using System.Threading.Tasks;

namespace Indigo.Client.Core.ViewModels
{
    public class ViewUserViewModel : BaseViewModel
	{
		User internalUser;
		public User user
		{
			get => internalUser;
			set => SetProperty(ref internalUser, value);
		}

		public bool newUser;

		public ViewUserViewModel(int? existingUserId = null)
		{
			newUser = !existingUserId.HasValue;
			user = newUser ? new User() : new User { UserId = existingUserId.Value };
		}

		public async Task GetExistingUser()
		{
			if (!newUser)
			{
				user = await Server.GetUserAsync(user.UserId);
			}
		}
	}
}