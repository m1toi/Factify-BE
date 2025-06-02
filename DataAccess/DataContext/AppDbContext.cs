using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.DataContext
{
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<UserInteraction> UserInteractions { get; set; }
		public DbSet<UserCategoryPreference> UserCategoryPreferences { get; set; }
		public DbSet<UserSeenPost> UserSeenPosts { get; set; }
		public DbSet<Friendship> Friendships { get; set; }
		public DbSet<Conversation> Conversations { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<Report> Reports { get; set; }


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

			// UserCategoryPreference - Composite Key
			modelBuilder.Entity<UserCategoryPreference>()
				.HasKey(ucp => new { ucp.UserId, ucp.CategoryId }); // Define composite primary key

			modelBuilder.Entity<UserCategoryPreference>()
				.HasOne(ucp => ucp.User)
				.WithMany(u => u.Preferences)
				.HasForeignKey(ucp => ucp.UserId);

			modelBuilder.Entity<UserCategoryPreference>()
				.HasOne(ucp => ucp.Category)
				.WithMany(c => c.UserPreferences)
				.HasForeignKey(ucp => ucp.CategoryId);

			// UserInteraction - Relationship & Composite Key
			modelBuilder.Entity<UserInteraction>()
				.HasKey(ui => ui.InteractionId); // Normal primary key

			modelBuilder.Entity<UserInteraction>()
				.HasOne(ui => ui.User)
				.WithMany(u => u.Interactions)
				.HasForeignKey(ui => ui.UserId)
				.OnDelete(DeleteBehavior.Restrict);  // Prevents cascading delete

			modelBuilder.Entity<UserInteraction>()
				.HasOne(ui => ui.Post)
				.WithMany(p => p.Interactions)
				.HasForeignKey(ui => ui.PostId)
				.OnDelete(DeleteBehavior.Cascade);

			// Category - Posts Relationship
			modelBuilder.Entity<Post>()
				.HasOne(p => p.Category)
				.WithMany(c => c.Posts)
				.HasForeignKey(p => p.CategoryId);

			modelBuilder.Entity<UserSeenPost>().ToTable("UserSeenPosts");

			modelBuilder.Entity<UserSeenPost>()
				.HasKey(usp => new { usp.UserId, usp.PostId });

			// Configure relationship from UserSeenPost to User
			modelBuilder.Entity<UserSeenPost>()
				.HasOne(usp => usp.User)
				.WithMany(u => u.SeenPosts)      // Ensure your User entity has a property: List<UserSeenPost> SeenPosts { get; set; }
				.HasForeignKey(usp => usp.UserId)
				.OnDelete(DeleteBehavior.Restrict); // or NO ACTION, depending on your design

			// Configure relationship from UserSeenPost to Post
			modelBuilder.Entity<UserSeenPost>()
				.HasOne(usp => usp.Post)
				.WithMany(p => p.UserSeenPosts)  // Ensure your Post entity has a property: List<UserSeenPost> UserSeenPosts { get; set; }
				.HasForeignKey(usp => usp.PostId)
				.OnDelete(DeleteBehavior.Cascade);

			// FRIENDSHIP relations:
			modelBuilder.Entity<Friendship>()
				.HasOne(f => f.User)
				.WithMany(u => u.FriendshipsInitiated)
				.HasForeignKey(f => f.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Friendship>()
				.HasOne(f => f.Friend)
				.WithMany(u => u.FriendshipsReceived)
				.HasForeignKey(f => f.FriendId)
				.OnDelete(DeleteBehavior.Restrict);

			// 🔹 CONVERSATION relations:
			modelBuilder.Entity<Conversation>()
				.HasOne(c => c.User1)
				.WithMany(u => u.ConversationsAsUser1)
				.HasForeignKey(c => c.User1Id)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Conversation>()
				.HasOne(c => c.User2)
				.WithMany(u => u.ConversationsAsUser2)
				.HasForeignKey(c => c.User2Id)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Conversation>()
				.HasIndex(c => new { c.User1Id, c.User2Id })
				.IsUnique();

			// 🔹 MESSAGE relations:
			modelBuilder.Entity<Message>()
				.HasOne(m => m.Conversation)
				.WithMany(c => c.Messages)
				.HasForeignKey(m => m.ConversationId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Message>()
				.HasOne(m => m.Sender)
				.WithMany(u => u.SentMessages)
				.HasForeignKey(m => m.SenderId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Message>()
				.HasOne(m => m.Post)
				.WithMany()
				.HasForeignKey(m => m.PostId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Notification>()
				.HasOne(n => n.FromUser)
				.WithMany(u => u.SentNotifications)
				.HasForeignKey(n => n.FromUserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Notification>()
				.HasOne(n => n.ToUser)
				.WithMany(u => u.ReceivedNotifications)
				.HasForeignKey(n => n.ToUserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Notification>()
				.Property(n => n.Type)
				.HasConversion<string>();

			// ─── REPORT relations ─────────────────────────────────
			modelBuilder.Entity<Report>()
				.HasOne(r => r.Post)
				.WithMany(p => p.Reports)
				.HasForeignKey(r => r.PostId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Report>()
				.HasOne(r => r.ReporterUser)
				.WithMany(u => u.ReportsMade)
				.HasForeignKey(r => r.ReporterUserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Report>()
				.HasOne(r => r.AdminUser)
				.WithMany(u => u.ReportsResolved)
				.HasForeignKey(r => r.AdminUserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Report>()
				.Property(r => r.Reason)
				.HasConversion<string>();

			modelBuilder.Entity<Report>()
				.Property(r => r.Status)
				.HasConversion<string>();

			modelBuilder.Entity<Role>().HasData(
				new Role { RoleId = 1, Name = "Admin" },
				new Role { RoleId = 2, Name = "User" }
			);
		}
	}
}
