using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.CategoryRepository
{
	public class CategoryRepository : BaseRepository, ICategoryRepository
	{
		public CategoryRepository(AppDbContext context) : base(context)
		{
		}
		public List<Category> GetAll()
		{
			return _context.Categories.ToList();
		}
		public Category Get(int id)
		{
			var category = _context.Categories.Find(id);
			if (category == null) 
			{
				throw new Exception("Category not found");
			}
			return category;
		}
		public void Create(Category category)
		{
			if(_context.Categories.Any(c => c.Name == category.Name))
			{
				throw new Exception($"Category with name {category.Name} already exists");
			}
			_context.Categories.Add(category);
			SaveChanges();
		}
		public void Update(int id, Category category)
		{
			var updatedCategory = _context.Categories.Find(id);
			if (updatedCategory == null)
			{
				throw new Exception("Category not found");
			}
			if (_context.Categories.Any(c => c.Name == updatedCategory.Name && c.CategoryId != id))
			{
				throw new Exception($"Category {updatedCategory} already exists");
			}
			updatedCategory.Name = category.Name;
			SaveChanges();
		}
		public void Delete(int id)
		{
			var categoryToDelete = _context.Categories.Include(c => c.Posts)
														.FirstOrDefault(c => c.CategoryId == id);
			if(categoryToDelete == null)
			{
				throw new Exception($"Category with ID {id} not found");
			}
			if (categoryToDelete.Posts.Count() > 0)
			{
				throw new Exception($"Category with ID {id} has posts");
			}
			_context.Categories.Remove(categoryToDelete);
			SaveChanges();
		}
	}
}
