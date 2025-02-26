using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Repositories.CategoryRepository;
using SocialMediaApp.DataAccess.Repositories.PostRepository;
using SocialMediaApp.DataAccess.Repositories.UserCategoryRepository;
using SocialMediaApp.DataAccess.Repositories.UserInteractionRepository;

namespace SocialMediaApp.BusinessLogic.Services
{
	public class FeedService : IFeedService
	{
		private readonly IPostRepository _postRepository;
		private readonly IUserCategoryRepository _userCategoryRepository;
		private readonly IUserInteractionRepository _userInteractionRepository;
		private readonly ICategoryRepository _categoryRepository;

		public FeedService(IPostRepository postRepository,
			IUserCategoryRepository userCategoryRepository,
			IUserInteractionRepository userInteractionRepository,
			ICategoryRepository categoryRepository)
		{
			_postRepository = postRepository;
			_userCategoryRepository = userCategoryRepository;
			_userInteractionRepository = userInteractionRepository;
			_categoryRepository = categoryRepository;
		}

		public List<Post> GetPersonalizedFeed(int userId, int totalPosts = 20)
		{
			//Determine the percentage of posts 
			int preferredCount = (int)(totalPosts * 0.8);
			int nonPreferredCount = totalPosts - preferredCount;
			//Get the user's preferred categories
			var userCategoryPreferences = _userCategoryRepository.GetPreferencesByUser(userId);
			var preferredCategoryIds = userCategoryPreferences
				.Where(ucp => ucp.Score > 0)
				.Select(ucp => ucp.CategoryId)
				.ToList();

			var categoryIds = _categoryRepository.GetAll()
				.Select(c => c.CategoryId)
				.ToList();
			var nonPreferredCategoryIds = categoryIds.Except(preferredCategoryIds).ToList();

			var preferredPosts = _postRepository.GetPostsByCategories(preferredCategoryIds, preferredCount);
			var nonPreferredPosts = _postRepository.GetPostsByCategories(nonPreferredCategoryIds, nonPreferredCount);

			var combinedPosts=preferredPosts.Concat(nonPreferredPosts)
											.OrderByDescending(p => p.CreatedAt)
											.ToList();
			return combinedPosts; 
		}
	}
}
