using System.Collections.Generic;

namespace Indigo.Core.Models
{
    public class Conversation
    {
		public int ConversationId;
		public string ConversationName;
		public bool isGroupChat;
		public ICollection<UserConversation> UserConversations;
		public ICollection<Message> Messages;
	}
}