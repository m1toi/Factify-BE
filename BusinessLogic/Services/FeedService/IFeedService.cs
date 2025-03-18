using SocialMediaApp.DataAccess.Dtos.PostDto;

namespace SocialMediaApp.BusinessLogic.Services.FeedService
{
	public interface IFeedService
	{
		public List<PostResponseDto> GetPersonalizedFeed(int userId, int totalPosts = 20); 
	}
}
