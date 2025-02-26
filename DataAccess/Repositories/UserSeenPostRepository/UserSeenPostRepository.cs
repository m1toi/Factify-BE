using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.UserSeenPostRepository
{
	public class UserSeenPostRepository : BaseRepository, IUserSeenPostRepository
	{
		public UserSeenPostRepository(AppDbContext context) : base(context)
		{
		}

		public void Add(UserSeenPost userSeenPost)
		{
			var existingUserSeenPost = _context.UserSeenPosts
				.FirstOrDefault(usp => usp.UserId == userSeenPost.UserId && usp.PostId == userSeenPost.PostId);
			if (existingUserSeenPost != null)
			{
				throw new Exception("User seen post already exists");
			}
			userSeenPost.SeenAt = DateTime.Now;
			_context.UserSeenPosts.Add(userSeenPost);
			SaveChanges();
		}

		public List<int> GetSeenPostIds(int userId)
		{
			return _context.UserSeenPosts
				.Where(usp => usp.UserId == userId)
				.Select(usp => usp.PostId)
				.ToList();
		}
	}
}
