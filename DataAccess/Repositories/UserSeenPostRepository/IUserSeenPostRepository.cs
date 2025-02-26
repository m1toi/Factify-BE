using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.UserSeenPostRepository
{
	public interface IUserSeenPostRepository
	{
		void Add(UserSeenPost userSeenPost);
		List<int> GetSeenPostIds(int userId);
	}
}
