namespace SocialMediaApp.DataAccess.Dtos.ConversationDto
{
	public class ConversationUpdatedDto
	{
		public int ConversationId { get; set; }
		public string LastMessage { get; set; } = null!;
		public int LastMessageSenderId { get; set; }
		public DateTimeOffset LastMessageSentAt { get; set; }
	}
}
