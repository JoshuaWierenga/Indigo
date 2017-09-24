using Indigo.Core.Models;

namespace Indigo.Client.Core.ViewModels
{
    public class NewUserViewModel : BaseViewModel
	{
		public User user { get; set; }

		string usernameText;
		public string UsernameText
		{
			get => usernameText;
			set => SetProperty(ref usernameText, value);
		}

		string passwordHashText;
		public string PasswordHashText
		{
			get => passwordHashText;
			set => SetProperty(ref passwordHashText, value);
		}

		string emailText;
		public string EmailText
		{
			get => emailText;
			set => SetProperty(ref emailText, value);
		}

		public NewUserViewModel()
		{
			user = new User();
		}
	}
}