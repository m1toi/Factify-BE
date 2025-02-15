using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.AppDbContext
{
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Post> Posts { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
				.HasOne(u => u.Role)
				.WithMany(r => r.Users)
				.HasForeignKey(u => u.RoleId);

			modelBuilder.Entity<Post>()
				.HasOne(p => p.User)
				.WithMany(u => u.Posts)
				.HasForeignKey(p => p.UserId);

			modelBuilder.Entity<Role>().HasData(
				new Role { RoleId = 1, Name = "Admin" },
				new Role { RoleId = 2, Name = "User" }
			);
		}
	}
}
