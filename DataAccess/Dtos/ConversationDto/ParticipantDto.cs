namespace SocialMediaApp.DataAccess.Dtos.ConversationDto
{
	public class ParticipantDto
	{
		public int UserId { get; set; }
		public string Username { get; set; } = null!;
		public string? ProfilePicture { get; set; }
	}
}
