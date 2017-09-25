using Indigo.Core.Models;

namespace Indigo.Client.Core.ViewModels
{
    public class ModifyUserViewModel : ViewUserViewModel
	{
		string usernameExtraText;
		public string UsernameExtraText
		{
			get => usernameExtraText;
			set => SetProperty(ref usernameExtraText, value);
		}

		string passwordHashExtraText;
		public string PasswordHashExtraText
		{
			get => passwordHashExtraText;
			set => SetProperty(ref passwordHashExtraText, value);
		}

		string emailExtraText;
		public string EmailExtraText
		{
			get => emailExtraText;
			set => SetProperty(ref emailExtraText, value);
		}

		public string SaveText => newUser ? "Create" : "Save";

		public ModifyUserViewModel(int? existingUserId = null)
		{
			newUser = !existingUserId.HasValue;
			user = newUser ? new User() : new User { UserId = existingUserId.Value };
		}

		public bool ValidateInput(string input)
		{
			string inputValue = user.GetType().GetProperty(input).GetValue(user) as string;
			bool valid = !string.IsNullOrWhiteSpace(inputValue);

			GetType().GetProperty(input + "ExtraText").SetValue(this, !valid ? "Can not be empty" : "");
			return valid;
		}
	}
}