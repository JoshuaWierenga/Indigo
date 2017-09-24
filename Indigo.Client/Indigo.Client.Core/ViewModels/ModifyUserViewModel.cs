using Indigo.Core.Models;

namespace Indigo.Client.Core.ViewModels
{
    public class ModifyUserViewModel : BaseViewModel
	{
		User internalUser;
		public User user
		{
			get => internalUser;
			set => SetProperty(ref internalUser, value);
		}

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

		public bool newUser;
		public string SaveText => newUser ? "Create" : "Save";

		public ModifyUserViewModel(User existingUser = null)
		{
			newUser = existingUser == null;
			user = existingUser ?? new User();
		}

		public bool ValidateInput(string input)
		{
			string inputValue = user.GetType().GetProperty(input).GetValue(user) as string;
			bool valid = !string.IsNullOrWhiteSpace(inputValue);

			GetType().GetProperty(input + "Text").SetValue(this, !valid ? "Can not be empty" : "");
			return valid;
		}
	}
}