namespace SocialMediaApp.DataAccess.Dtos.FriendshipDto
{
	public class FriendshipResponseDto
	{
		public int FriendshipId { get; set; }
		public int UserId { get; set; }
		public string Username { get; set; }
		public int FriendId { get; set; }
		public string FriendUsername { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsConfirmed { get; set; }
	}
}
