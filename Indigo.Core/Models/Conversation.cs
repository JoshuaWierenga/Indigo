using System.Collections.Generic;

namespace Indigo.Core.Models
{
    public class Conversation
    {
		public int ConversationId { get; set; }
		public string ConversationName { get; set; }
		public bool isGroupChat { get; set; }
		public List<UserConversation> UserConversations { get; set; }
		public ICollection<Message> Messages { get; set; }
	}
}