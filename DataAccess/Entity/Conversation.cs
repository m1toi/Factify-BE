namespace SocialMediaApp.DataAccess.Entity
{
	public class Conversation
	{
		public int ConversationId { get; set; }

		public int User1Id { get; set; }
		public User User1 { get; set; }

		public int User2Id { get; set; }
		public User User2 { get; set; }

		public DateTime CreatedAt { get; set; }

		public List<Message> Messages { get; set; }
	}
}
