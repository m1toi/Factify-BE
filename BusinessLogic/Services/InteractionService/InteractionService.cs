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
		public InteractionService(
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
		//NOT IN USE
		public void HandleInteraction(int userId, int postId, bool liked, bool shared)
		{
			var existingInteraction = _userInteractionRepository.GetAll()
						.FirstOrDefault(i => i.User.UserId == userId && i.PostId == postId);

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

			var post = _postRepository.Get(postId);
			int categoryId = post.CategoryId;
			double scoreAdjustment = 0;
			if (liked)
			{
				scoreAdjustment += 0.5;
			}
			if (shared)
			{
				scoreAdjustment += 1.0;
			}
			if (!liked && !shared)
			{
				scoreAdjustment -= 0.1;
			}
			try
			{
				_userCategoryRepository.Update(userId, categoryId, scoreAdjustment);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("not found"))
				{
					_userCategoryRepository.Add(new UserCategoryPreference
					{
						UserId = userId,
						CategoryId = categoryId,
						Score = scoreAdjustment
					});
				}
				else
				{
					Console.WriteLine(ex.Message);
				}
			}
		}

		public void ToggleLike(int userId, int postId)
		{
			var existing = _userInteractionRepository.GetAll()
				.FirstOrDefault(i => i.UserId == userId && i.PostId == postId);

			var post = _postRepository.Get(postId);
			var categoryId = post.CategoryId;

			if (existing == null)
			{
				_userInteractionRepository.Add(new UserInteraction
				{
					UserId = userId,
					PostId = postId,
					Liked = true,
					Shared = false,
					InteractionDate = DateTime.UtcNow
				});
				_userCategoryRepository.Update(userId, categoryId, 0.5);
			}
			else
			{
				existing.Liked = !existing.Liked;
				existing.InteractionDate = DateTime.UtcNow;

				_userCategoryRepository.Update(userId, categoryId, existing.Liked ? 0.5 : -0.5);
			}
		}

		public void SharePost(int userId, int postId)
		{
			var existing = _userInteractionRepository.GetAll()
				.FirstOrDefault(i => i.UserId == userId && i.PostId == postId);

			var post = _postRepository.Get(postId);
			var categoryId = post.CategoryId;

			if (existing == null)
			{
				_userInteractionRepository.Add(new UserInteraction
				{
					UserId = userId,
					PostId = postId,
					Liked = false,
					Shared = true,
					InteractionDate = DateTime.UtcNow
				});
			}
			else if (!existing.Shared)
			{
				existing.Shared = true;
				existing.InteractionDate = DateTime.UtcNow;
			}

			_userCategoryRepository.Update(userId, categoryId, 1.0);
		}

		public void MarkPostAsSeen(int userId, int postId)
		{
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
				Console.WriteLine($"Post already marked as seen: {ex.Message}");
			}
		}

		// NOT IN USE FOR NOW
		public void MarkNotInterested(int userId, int postId)
		{
			var post = _postRepository.Get(postId);
			int categoryId = post.CategoryId;

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
				Console.WriteLine($"Post already marked as seen: {ex.Message}");
			}

			try
			{
				_userCategoryRepository.Update(userId, categoryId, -0.3); 
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("not found"))
				{
					_userCategoryRepository.Add(new UserCategoryPreference
					{
						UserId = userId,
						CategoryId = categoryId,
						Score = -0.3
					});
				}
				else
				{
					Console.WriteLine(ex.Message);
				}
			}
		}


	}
}
