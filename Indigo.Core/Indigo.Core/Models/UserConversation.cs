namespace Indigo.Core.Models
{
    public class UserConversation
    {
		public int UserConversationId;
		public int UserId;
		public int ConversationId;
		public Conversation Conversation;
		public User User;
		public bool isAdmin;
    }
}