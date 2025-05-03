namespace SocialMediaApp.DataAccess.Entity
{
	public class Message
	{
		public int MessageId { get; set; }

		public int ConversationId { get; set; }
		public Conversation Conversation { get; set; }

		public int SenderId { get; set; }
		public User Sender { get; set; }

		public string Content { get; set; }

		public int? PostId { get; set; }  // Nullable: message might or might not contain a post
		public Post Post { get; set; }

		public DateTime SentAt { get; set; }

		public bool IsRead { get; set; }
	}
}
