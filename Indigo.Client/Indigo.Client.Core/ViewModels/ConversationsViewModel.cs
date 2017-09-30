using Indigo.Core.Models;

namespace Indigo.Client.Core.ViewModels
{
    public class ConversationsViewModel : BaseViewModel
    {
		User internalUser;
		public User User
		{
			get => internalUser;
			set => SetProperty(ref internalUser, value);
		}

		public ConversationsViewModel(User existingUser)
		{
			User = existingUser;
		}
	}
}