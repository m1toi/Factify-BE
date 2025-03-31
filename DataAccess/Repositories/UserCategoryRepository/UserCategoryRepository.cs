using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.UserCategoryRepository
{
	public class UserCategoryRepository : BaseRepository, IUserCategoryRepository
	{
		public UserCategoryRepository(AppDbContext context) : base(context)
		{
		}

		public void Add(UserCategoryPreference userCategoryPreference)
		{
			if(_context.UserCategoryPreferences.Any(ucp => ucp.UserId == userCategoryPreference.UserId 
			&&ucp.CategoryId == userCategoryPreference.CategoryId))
			{
				throw new Exception("User category preference already exists");
			}
			_context.UserCategoryPreferences.Add(userCategoryPreference);
			SaveChanges();
		}

		public void Delete(int userId, int categoryId)
		{
			var userCategoryPreference = _context.UserCategoryPreferences
				.FirstOrDefault(ucp => ucp.UserId == userId && ucp.CategoryId == categoryId);
			if (userCategoryPreference == null)
			{
				throw new Exception("User category preference not found");
			}
			_context.UserCategoryPreferences.Remove(userCategoryPreference);
			SaveChanges();
		}

		public UserCategoryPreference Get(int userId, int categoryId)
		{
			var userCategoryPreference = _context.UserCategoryPreferences
				.FirstOrDefault(ucp => ucp.UserId == userId && ucp.CategoryId == categoryId);

			//if(userCategoryPreference == null)
			//{
			//	throw new Exception("User category preference not found");
			//}
			return userCategoryPreference;
		}

		public List<UserCategoryPreference> GetAll()
		{
			return _context.UserCategoryPreferences.ToList();
		}

		public List<UserCategoryPreference> GetPreferencesByUser(int userId)
		{
			return _context.UserCategoryPreferences
				.Where(ucp => ucp.UserId == userId)
				.ToList();
		}

		public void Update(int userId, int categoryId, double score)
		{
			var userCategoryPreference = _context.UserCategoryPreferences
				.FirstOrDefault(ucp => ucp.UserId == userId && ucp.CategoryId == categoryId);
			if (userCategoryPreference == null)
			{
				throw new Exception("User category preference not found");
			}
			userCategoryPreference.Score += score;
			SaveChanges();
		}
	}
}
