namespace SocialMediaApp.DataAccess.Dtos.ConversationDto
{
	public class ConversationReadDto
	{
		public int ConversationId { get; set; }
		public int UnreadCount { get; set; }
		public bool HasUnread { get; set; }
	}
}
