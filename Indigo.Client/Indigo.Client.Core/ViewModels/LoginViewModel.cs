namespace Indigo.Client.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
		string username;
		public string Username
		{
			get => username;
			set => SetProperty(ref username, value);
		}

		string passwordHash;
		public string PasswordHash
		{
			get => passwordHash;
			set => SetProperty(ref passwordHash, value);
		}
	}
}