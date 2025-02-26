using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.BusinessLogic.Services
{
	public interface IFeedService
	{
		public List<Post> GetPersonalizedFeed(int userId, int totalPosts = 20); 
	}
}
