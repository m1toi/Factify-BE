using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Repositories;
using SocialMediaApp.DataAccess.Repositories.CategoryRepository;
using SocialMediaApp.DataAccess.Repositories.PostRepository;
using SocialMediaApp.DataAccess.Repositories.RoleRepository;
using SocialMediaApp.DataAccess.Repositories.UserCategoryRepository;
using SocialMediaApp.DataAccess.Repositories.UserInteractionRepository;
using SocialMediaApp.DataAccess.Repositories.UserRepository;

namespace SocialMediaApp.BusinessLogic.Extensions
{
	public static class ServicesExtension
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
		}
		public static void ConfigureDbContext(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<AppDbContext>(
			   options => options.UseSqlServer(connectionString));
		}


	}
}
