using Indigo.Core.Models;

namespace Indigo.Client.Core.ViewModels
{
    public class TestLoggedIn : BaseViewModel
    {
		User internalUser;
		public User User
		{
			get => internalUser;
			set => SetProperty(ref internalUser, value);
		}

		public TestLoggedIn(User existingUser)
		{
			User = existingUser;
		}
	}
}
