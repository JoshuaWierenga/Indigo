using Indigo.Core.Models;

namespace Indigo.Client.Core.ViewModels
{
    public class ConversationsViewModel : BaseViewModel
    {
		User _User;
		public User User
		{
			get =>_User;
			set => SetProperty(ref _User, value);
		}

		public ConversationsViewModel(User existingUser)
		{
			User = existingUser;
		}
	}
}