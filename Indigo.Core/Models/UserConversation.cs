namespace Indigo.Core.Models
{
    public class UserConversation
	{
		public int UserId { get; set; }
		public int ConversationId { get; set; }
		public Conversation Conversation { get; set; }
		public User User { get; set; }
		public bool isAdmin { get; set; }
    }
}