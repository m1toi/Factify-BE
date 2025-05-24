using SocialMediaApp.DataAccess.Dtos.PostDto;

namespace SocialMediaApp.DataAccess.Dtos.MessageDto
{
	public class MessageResponseDto
	{
		public int MessageId { get; set; }
		public int ConversationId { get; set; }
		public int SenderId { get; set; }
		public string SenderUsername { get; set; }
		public string? SenderProfilePicture { get; set; }
		public string Content { get; set; }
		public int? PostId { get; set; }
		public PostResponseDto? Post { get; set; }
		public DateTime SentAt { get; set; }
	}
}
