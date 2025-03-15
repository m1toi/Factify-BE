using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.PostRepository
{
	public interface IPostRepository
	{
		List<Post> GetAll();
		Post Get(int id);
		List<Post> GetPostsByCategoriesExcludingSeen(List<int> categoryIds, List<int> seenPostIds, int totalPosts = 20);
		Post Create(Post post);
		Post Update(int id, Post post);
		void Delete(int id);
	}
}
