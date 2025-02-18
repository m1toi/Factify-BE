namespace SocialMediaApp.DataAccess.Entity
{
	public class UserCategoryPreference
	{
		public int UserId { get; set; }
		public int CategoryId { get; set; }
		public double Score { get; set; }
		public User User { get; set; }
		public Category Category { get; set; }
	}
}
