using System;

namespace Indigo.Core.Models
{
    public class Message
    {
		public int MessageId { get; set; }
		public string message { get; set; }
		public DateTime CreationTime { get; set; }
		public bool isEdited { get; set; }
		public User Sender { get; set; }
		public Conversation Location { get; set; }
	}
}