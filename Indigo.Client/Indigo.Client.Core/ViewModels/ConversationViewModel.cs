using Indigo.Core.Models;
using System.Threading.Tasks;

namespace Indigo.Client.Core.ViewModels
{
    class ConversationViewModel : ConversationsViewModel
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

		public ConversationViewModel(User existingUser, Conversation existingConversation = null) : base(existingUser)
		{
			NewConversation = existingConversation == null;
			Conversation =  NewConversation ? existingConversation : new Conversation();
		}

		//TODO handle editing existing conversation
		public async Task SaveConversation(User ConversationPartner)
		{
			if (NewConversation)
			{
				Conversation newConversation = await Server.CreateConversationAsync(User, Conversation);

				await Server.CreateUserConversationAsync(User, User, Conversation);
				await Server.CreateUserConversationAsync(User, ConversationPartner, Conversation);
			}
		}
	}
}