using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.PostRepository
{
	public interface IPostRepository
	{
		List<Post> GetAll();
		Post Get(int id);
		List<Post> GetByUser(int userId, int page, int pageSize);
		List<Post> GetPostsByCategoryExcludingSeen(int categoryId, List<int> seenPostIds, int count);
		Post Create(Post post);
		Post Update(int id, Post post);
		void Delete(int id);
	}
}
