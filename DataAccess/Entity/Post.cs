namespace SocialMediaApp.DataAccess.Entity
{
	public class Post
	{
		public int PostId { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
		public DateTime CreatedAt { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
