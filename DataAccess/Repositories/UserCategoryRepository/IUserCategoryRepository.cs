using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.UserCategoryRepository
{
	public interface IUserCategoryRepository
	{
		public List<UserCategoryPreference> GetAll();
		public UserCategoryPreference Get(int userId, int categoryId);	
		public List<UserCategoryPreference> GetPreferencesByUser(int userId);
		public void Add(UserCategoryPreference userCategoryPreference);
		public void Update(int userId, int categoryId, double score);
		public void Delete(int userId, int categoryId);
	}
}
