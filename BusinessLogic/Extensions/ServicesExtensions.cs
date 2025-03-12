using Microsoft.EntityFrameworkCore;
using SocialMediaApp.BusinessLogic.Services.CategoryService;
using SocialMediaApp.BusinessLogic.Services.FeedService;
using SocialMediaApp.BusinessLogic.Services.InteractionService;
using SocialMediaApp.BusinessLogic.Services.PostService;
using SocialMediaApp.BusinessLogic.Services.UserService;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Repositories;
using SocialMediaApp.DataAccess.Repositories.CategoryRepository;
using SocialMediaApp.DataAccess.Repositories.PostRepository;
using SocialMediaApp.DataAccess.Repositories.RoleRepository;
using SocialMediaApp.DataAccess.Repositories.UserCategoryRepository;
using SocialMediaApp.DataAccess.Repositories.UserInteractionRepository;
using SocialMediaApp.DataAccess.Repositories.UserRepository;
using SocialMediaApp.DataAccess.Repositories.UserSeenPostRepository;

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

			//Configure services
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<IInteractionService, InteractionService>();
			services.AddScoped<IFeedService, FeedService>();
		}
		public static void ConfigureDbContext(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<AppDbContext>(
			   options => options.UseSqlServer(connectionString));
		}
	}
}
