using SocialMediaApp.DataAccess.DataContext;

namespace SocialMediaApp.DataAccess.Repositories
{
	public class BaseRepository
	{
		protected readonly AppDbContext _context;

		public BaseRepository(AppDbContext context)
		{
			_context = context;
		}

		public void SaveChanges()
		{
			_context.SaveChanges();
		}
	}
}
