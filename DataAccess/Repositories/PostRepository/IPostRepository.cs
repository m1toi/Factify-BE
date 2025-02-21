using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.PostRepository
{
	public interface IPostRepository
	{
		List<Post> GetAll();
		Post Get(int id);
		void Create(Post post);
		void Update(int id, Post post);
		void Delete(int id);
	}
}
