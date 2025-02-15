using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;

namespace SocialMediaApp.BusinessLogic.Extensions
{
	public static class ServicesExtension
	{
		public static void ConfigureDbContext(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<AppDbContext>(
			   options => options.UseSqlServer(connectionString));
		}


	}
}
