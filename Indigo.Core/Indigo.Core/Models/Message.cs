using System;

namespace Indigo.Core.Models
{
    public class Message
    {
		public int MessageId;
		public string message;
		public DateTime CreationTime;
		public bool isEdited;
		public User Sender;
		public Conversation Location;
	}
}