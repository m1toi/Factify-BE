namespace SocialMediaApp.DataAccess.Entity
{
	public class User
	{
		public int UserId { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string ProfilePicture { get; set; }
		public int RoleId { get; set; }
		public Role Role { get; set; }
		public List<Post> Posts { get; set; }
		public List<UserSeenPost> SeenPosts { get; set; }
		public List<UserInteraction> Interactions { get; set; }
		public List<UserCategoryPreference> Preferences { get; set; }
	}
}
