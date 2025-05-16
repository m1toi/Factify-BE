using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.UserRepository
{
	public interface IUserRepository
	{
		void Register(User user);
		User GetByEmail(string email);
		List<User> SearchByUsername(string query, int excludeUserId = 0);
		List<User> GetAll();
		User Get(int id);
		void Update(int id, User user);
		void Delete(int id);
	}
}
