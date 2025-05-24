namespace SocialMediaApp.DataAccess.Dtos.MessageDto
{
	public class MessageRequestDto
	{
		public int ConversationId { get; set; }
		public int SenderId { get; set; }
		public string? Content { get; set; }
		public int? PostId { get; set; } // optional: sending a post inside the message
	}
}
