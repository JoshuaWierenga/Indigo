using Indigo.Core.Models;
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

		//TODO handle editing existing conversation
		string _partnerUsername;
		public string PartnerUsername
		{
			get => _partnerUsername;
			set => SetProperty(ref _partnerUsername, value);
		}

		string _conversationNameExtraText;
		public string ConversationNameExtraText
		{
			get => _conversationNameExtraText;
			set => SetProperty(ref _conversationNameExtraText, value);
		}

		string _conversationPartnerExtraText;
		public string ConversationPartnerExtraText
		{
			get => _conversationPartnerExtraText;
			set => SetProperty(ref _conversationPartnerExtraText, value);
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
			Conversation =  NewConversation ? new Conversation() : existingConversation;
		}

		//TODO handle editing existing conversation
		//TODO handle group chats
		public async Task SaveConversation()
		{
			if (NewConversation)
			{
				Conversation newConversation = await Server.CreateConversationAsync(User, Conversation);
				await Server.CreateUserConversationAsync(User, newConversation, await GetPartner(), true);
			}
		}

		async Task<User> GetPartner()
		{
			return await Server.GetUserAsync(User, PartnerUsername);
		}
	}
}