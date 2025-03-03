using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Repositories.PostRepository;
using SocialMediaApp.DataAccess.Repositories.UserCategoryRepository;
using SocialMediaApp.DataAccess.Repositories.UserInteractionRepository;
using SocialMediaApp.DataAccess.Repositories.UserSeenPostRepository;

namespace SocialMediaApp.BusinessLogic.Services.InteractionService
{
	public class InteractionService : IInteractionService
	{
		private readonly IUserInteractionRepository _userInteractionRepository;
		private readonly IPostRepository _postRepository;
		private readonly IUserSeenPostRepository _userSeenPostRepository;
		private readonly IUserCategoryRepository _userCategoryRepository;
		private InteractionService(
			IUserInteractionRepository userInteractionRepository,
			IPostRepository postRepository,
			IUserSeenPostRepository userSeenPostRepository,
			IUserCategoryRepository userCategoryRepository)
		{
			_userInteractionRepository = userInteractionRepository;
			_postRepository = postRepository;
			_userSeenPostRepository = userSeenPostRepository;
			_userCategoryRepository = userCategoryRepository;
		}
		public void HandleInteraction(int userId, int postId, bool liked, bool shared)
		{
			var existingInteraction = _userInteractionRepository.GetAll()
										.FirstOrDefault(i => i.User.UserId == userId);
			if (existingInteraction == null)
			{
				existingInteraction = new UserInteraction
				{
					UserId = userId,
					PostId = postId,
					Liked = liked,
					Shared = shared,
					InteractionDate = DateTime.UtcNow
				};
				_userInteractionRepository.Add(existingInteraction);

			}
			else
			{
				existingInteraction.Liked = liked;
				existingInteraction.Shared = shared;
				existingInteraction.InteractionDate = DateTime.UtcNow;

			}

			try
			{
				_userSeenPostRepository.Add(new UserSeenPost
				{
					UserId = userId,
					PostId = postId
				});
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			var post = _postRepository.Get(postId);


		}
	}
}
