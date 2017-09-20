using System.Collections.Generic;

namespace Indigo.Core.Models
{
    public class User
    {
		public int UserId;
		public string Username;
		public string PasswordHash;
		public string Email;
		public ICollection<UserConversation> UserConversations;
    }
}