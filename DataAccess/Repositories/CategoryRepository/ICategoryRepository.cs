using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.CategoryRepository
{
	public interface ICategoryRepository
	{
		List<Category> GetAll();
		Category Get(int id);
		void Create(Category category);
		void Update(int id, Category category);
		void Delete(int id);
	}
}
