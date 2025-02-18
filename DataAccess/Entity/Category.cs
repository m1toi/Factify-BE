namespace SocialMediaApp.DataAccess.Entity
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public List<Post> Posts { get; set; }
		public List<UserCategoryPreference> Preferences { get; set; }
	}
}
