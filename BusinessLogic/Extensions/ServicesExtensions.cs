using Microsoft.EntityFrameworkCore;
using SocialMediaApp.BusinessLogic.Services.CategoryService;
using SocialMediaApp.BusinessLogic.Services.FeedService;
using SocialMediaApp.BusinessLogic.Services.InteractionService;
using SocialMediaApp.BusinessLogic.Services.PostService;
using SocialMediaApp.BusinessLogic.Services.UserService;
using SocialMediaApp.BusinessLogic.Services.AuthenticationService;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Repositories;
using SocialMediaApp.DataAccess.Repositories.CategoryRepository;
using SocialMediaApp.DataAccess.Repositories.PostRepository;
using SocialMediaApp.DataAccess.Repositories.RoleRepository;
using SocialMediaApp.DataAccess.Repositories.UserCategoryRepository;
using SocialMediaApp.DataAccess.Repositories.UserInteractionRepository;
using SocialMediaApp.DataAccess.Repositories.UserRepository;
using SocialMediaApp.DataAccess.Repositories.UserSeenPostRepository;
using SocialMediaApp.BusinessLogic.Services.UserPreferenceService;
using SocialMediaApp.DataAccess.Repositories.MessageRepository;
using SocialMediaApp.DataAccess.Repositories.ConversationRepository;
using SocialMediaApp.DataAccess.Repositories.FriendshipRepository;
using SocialMediaApp.BusinessLogic.Services.MessageService;
using SocialMediaApp.BusinessLogic.Services.ConversationService;
using SocialMediaApp.BusinessLogic.Services.FriendshipService;
using SocialMediaApp.BusinessLogic.Services.NotificationService;
using SocialMediaApp.DataAccess.Repositories.NotificationRepository;

namespace SocialMediaApp.BusinessLogic.Extensions
{
	public static class ServicesExtensions
	{

		public static void AddBusinessService(this IServiceCollection services)
		{
			//Configure repositories
			services.AddScoped<BaseRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IPostRepository, PostRepository>();
			services.AddScoped<IUserCategoryRepository, UserCategoryRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<IUserInteractionRepository, UserInteractionRepository>();
			services.AddScoped<IUserSeenPostRepository, UserSeenPostRepository>();
			services.AddScoped<IMessageRepository, MessageRepository>();
			services.AddScoped<IConversationRepository, ConversationRepository>();
			services.AddScoped<IFriendshipRepository, FriendshipRepository>(); 
			services.AddScoped<INotificationRepository, NotificationRepository>();


			//Configure services
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<IInteractionService, InteractionService>();
			services.AddScoped<IFeedService, FeedService>();
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<IUserPreferenceService, UserPreferenceService>();
			services.AddScoped<IMessageService, MessageService>();
			services.AddScoped<IConversationService, ConversationService>();
			services.AddScoped<IFriendshipService, FriendshipService>();
			services.AddScoped<INotificationService, NotificationService>();

		}
		public static void ConfigureDbContext(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<AppDbContext>(
			   options => options.UseSqlServer(connectionString));
		}
	}
}
