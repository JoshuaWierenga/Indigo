using System.Collections.Generic;

namespace Indigo.Core.Models
{
    public class User
    {
		public int UserId { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string Email { get; set; }
		public List<UserConversation> UserConversations { get; set; }
    }
}