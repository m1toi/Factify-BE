namespace SocialMediaApp.DataAccess.Dtos.FriendshipDto
{
	public class FriendForShareDto
	{
		public int UserId { get; set; }
		public string Username { get; set; } = null!;
		public string? ProfilePicture { get; set; }
		public DateTimeOffset LastChatAt { get; set; }
	}
}
