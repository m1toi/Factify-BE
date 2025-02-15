using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.RoleRepository
{
	public interface IRoleRepository
	{
		List<Role> GetAll();
		Role Get(int id);
		void Create(Role role);
		void Update(int id, Role role);
		void Delete(int id);
	}
}
