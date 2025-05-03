namespace SocialMediaApp.DataAccess.Entity
{
	public class UserSeenPost
	{
		// Composite key: UserId + PostId
		public int UserId { get; set; }
		public int PostId { get; set; }

		// When the post was seen
		public DateTime SeenAt { get; set; }

		// Navigation properties
		public User User { get; set; }
		public Post Post { get; set; }
	}
}
	