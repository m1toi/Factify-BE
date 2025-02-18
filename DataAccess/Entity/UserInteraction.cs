namespace SocialMediaApp.DataAccess.Entity
{
	public class UserInteraction
	{
		public int InteractionId { get; set; }
		public int PostId { get; set; }
		public int UserId { get; set; }

		public bool Liked { get; set; }
		public bool Shared { get; set; }

		public DateTime InteractionDate { get; set; }

		public User User { get; set; }
		public Post Post { get; set; }
	}
}
