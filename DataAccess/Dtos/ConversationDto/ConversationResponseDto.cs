namespace SocialMediaApp.DataAccess.Dtos.ConversationDto
{
	public class ConversationResponseDto
	{
		public int ConversationId { get; set; }
		public int User1Id { get; set; }
		public string User1Username { get; set; }
		public string? User1ProfilePicture { get; set; }
		public int User2Id { get; set; }
		public string User2Username { get; set; }
		public string? User2ProfilePicture { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public string? LastMessage { get; set; }
		public int? LastMessageSenderId { get; set; }
		public DateTimeOffset? LastMessageSentAt { get; set; }
	}
}
