using System.IO;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.UserRepository
{
	public class UserRepository : BaseRepository, IUserRepository
	{
		public UserRepository(AppDbContext context) : base(context)
		{
		}
		public List<User> GetAll()
		{
			var users = _context.Users
								.Include(u => u.Role)
								.ToList();
			return users;
		}

		public User GetByEmail(string email)
		{
			var user = _context.Users
				                .Where(u => u.Email == email)
								.Include(u => u.Role)
								.FirstOrDefault();
			if (user == null)
			{
				throw new Exception("User not found");
			}
			return user;
		}

		public List<User> SearchByUsername(string query, int excludeUserId = 0)
		{
			return _context.Users
				.Where(u => EF.Functions.Like(u.Username.ToLower(), $"%{query.ToLower()}%") &&
							u.UserId != excludeUserId)
				.OrderBy(u => u.Username)
				.Take(10)
				.ToList();
		}

		public User Get(int id)
		{
			var user = _context.Users
							   .Where(u => u.UserId == id)
							   .Include(u => u.Role)
							   .FirstOrDefault();
			if (user == null)
			{
				throw new Exception("User not found");
			}
			return user;
		}

		public void Register(User user)
		{
			if (_context.Users.Any(u => u.Email == user.Email))
			{
				throw new Exception($"User with {user.Email} already exists");
			}

			if (_context.Users.Any(u => u.Username == user.Username))
			{
				throw new Exception($"User with {user.Username} already exists");
			}

			var role = _context.Roles.Find(user.RoleId);
			if (role == null)
			{
				throw new Exception($"Role with ID {user.RoleId} not found");
			}

			_context.Add(user);
			SaveChanges();
		}

		public void Update(int id, User updatedUser)
		{
			var userToUpdate = _context.Users.Find(id);
			if (userToUpdate == null)
			{
				throw new Exception($"User with ID {id} not found");
			}

			var role = _context.Roles.Find(updatedUser.RoleId);
			if (role == null)
			{
				throw new Exception($"Role with ID {updatedUser.RoleId} not found");
			}

			if (_context.Users.Any(u => u.Email == updatedUser.Email && u.UserId != id))
			{
				throw new Exception($"User with {updatedUser.Email} already exists");
			}
			if (_context.Users.Any(u => u.Username == updatedUser.Username && u.UserId != id))
			{
				throw new Exception($"Username '{updatedUser.Username}' is already taken");
			}

			userToUpdate.Username = updatedUser.Username;
			userToUpdate.Email = updatedUser.Email;
			userToUpdate.Password = updatedUser.Password;
			userToUpdate.ProfilePicture = updatedUser.ProfilePicture;
			userToUpdate.RoleId = updatedUser.RoleId;
			SaveChanges();
		}

		public void Delete(int id)
		{
			var userToDelete = _context.Users.Find(id);
			if (userToDelete == null)
			{
				throw new Exception("User not found");
			}

			_context.Users.Remove(userToDelete);
			SaveChanges();
		}
	}
}
