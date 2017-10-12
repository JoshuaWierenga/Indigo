using Indigo.Core.Models;
using MvvmHelpers;
using System.Linq;
using System.Threading.Tasks;

namespace Indigo.Client.Core.ViewModels
{
	class ModifyConversationViewModel : ConversationsViewModel
	{
		Conversation _conversation;
		public Conversation Conversation
		{
			get => _conversation;
			set => SetProperty(ref _conversation, value);
		}

		ObservableRangeCollection<UserConversation> _partners;
		public ObservableRangeCollection<UserConversation> Partners
		{
			get => _partners;
			set => SetProperty(ref _partners, value);
		}

		bool _newConversation;
		public bool NewConversation
		{
			get => _newConversation;
			set => SetProperty(ref _newConversation, value);
		}

		public ModifyConversationViewModel(User existingUser, Conversation existingConversation = null) : base(existingUser)
		{
			NewConversation = existingConversation == null;
			Conversation = NewConversation ? new Conversation() : existingConversation;

			Partners = new ObservableRangeCollection<UserConversation>();

			if (NewConversation)
			{
				Partners.Add(new UserConversation
				{
					isAdmin = true
				});
			}
			else
			{
				Partners.AddRange(Conversation.UserConversations);
			}
		}

		//TODO handle group chats
		public async Task SaveConversation()
		{
			if (NewConversation)
			{
				Conversation = await Server.CreateConversationAsync(User, Conversation);
			}
			else
			{
				await Server.PutConversationAsync(User, Conversation);
			}

			if (Conversation.UserConversations.Count() == 0)
			{
				//TODO tell user to enter usernames 
				return;
			}
			else if (!Conversation.isGroupChat && Conversation.UserConversations.Count > 2)
			{
				//TOOD tell user that only one username can be entered for private chats
				return;
			}

			/*for (int i = 0; i < partners.Length; i++)
			{
				//TODO handle group chats
				bool userLevel = true;

				await Server.CreateUserConversationAsync(User, Conversation, await GetPartnerUser(partners[i].Trim()), userLevel);
			}*/
		}

		async Task<User> GetPartnerUser(string partnerUsername)
		{
			return await Server.GetUserAsync(User, partnerUsername);
		}

		public void AddPartner()
		{
			if (Conversation.isGroupChat || Partners.Count == 0)
			{
				Partners.Add(new UserConversation
				{
					isAdmin = false
				});
			}
		}

		public void ChangeChatType(bool newChatType)
		{
			Conversation temp = Conversation;
			temp.isGroupChat = true;
			Conversation = null;
			Conversation = temp;
		}
	}
}