namespace SocialMediaApp.DataAccess.Dtos.FriendshipDto
{
	public class FriendshipRequestDto
	{
		public int UserId { get; set; }
		public int FriendId { get; set; }
		public bool IsConfirmed { get; set; }
	}
}
