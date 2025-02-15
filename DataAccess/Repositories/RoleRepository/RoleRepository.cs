using Microsoft.EntityFrameworkCore;
using System.IO;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.RoleRepository
{
	public class RoleRepository : BaseRepository, IRoleRepository
	{
		public RoleRepository(AppDbContext context) : base(context)
		{
		}
		public List<Role> GetAll()
		{
			return _context.Roles.ToList();
		}
		public Role Get(int id)
		{
			var role = _context.Roles.Find(id);
			if (role == null)
			{
				throw new Exception("Role not found");
			}
			return role;
		}
		public void Create(Role role)
		{
			if (_context.Roles.Any(r => r.Name == role.Name))
			{
				throw new Exception($"Role with name {role.Name} already exists");
			}
			_context.Roles.Add(role);
			SaveChanges();
		}
		public void Update(int id, Role role)
		{
			var updatedRole = _context.Roles.Find(id);
			if (updatedRole == null)
			{
				throw new Exception("Role not found");
			}
			if (_context.Roles.Any(r => r.Name == updatedRole.Name && r.RoleId != id))
			{
				throw new Exception($"Role {updatedRole} already exists");
			}

			updatedRole.Name = updatedRole.Name;
			SaveChanges();

		}
		public void Delete(int id)
		{
			var roleToDelete = _context.Roles.Include(r => r.Users)
											 .FirstOrDefault(r => r.RoleId == id);
			if (roleToDelete == null)
			{
				throw new Exception($"Role with ID {id} not found");
			}

			if (roleToDelete.Users.Count() > 0)
			{
				throw new Exception($"Role with ID {id} has users");
			}

			_context.Roles.Remove(roleToDelete);
			SaveChanges();
		}
	}
}
