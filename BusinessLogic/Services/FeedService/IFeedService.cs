using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.BusinessLogic.Services.FeedService
{
	public interface IFeedService
	{
		public List<Post> GetPersonalizedFeed(int userId, int totalPosts = 20); 
	}
}
