namespace SocialMediaApp.DataAccess.Entity
{
	public class Friendship
	{
		public int FriendshipId { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }

		public int FriendId { get; set; }
		public User Friend { get; set; }

		public DateTime CreatedAt { get; set; }

		public bool IsConfirmed { get; set; }  // Set to true once both users accept 
	}
}
