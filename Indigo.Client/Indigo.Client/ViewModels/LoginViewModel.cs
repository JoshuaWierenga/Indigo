using System.Threading.Tasks;
using Indigo.Core.Models;

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

		public async Task<User> GetUserAsync()
		{
			User partialNewUser = new User
			{
				Username = Username,
				PasswordHash = Username
			};
			return  await Server.GetUserAsync(partialNewUser);
		}
	}
}