using Indigo.Core.Models;
using System.Threading.Tasks;

namespace Indigo.Client.ViewModels
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

		public async Task DeleteUserConversationAsync(Conversation conversation)
		{
			await Server.DeleteUserConversationAsync(User, conversation);
			User = await Server.GetUserAsync(User);
		}
	}
}