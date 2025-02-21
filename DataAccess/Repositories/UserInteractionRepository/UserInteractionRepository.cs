using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.UserInteractionRepository
{
	public class UserInteractionRepository : BaseRepository, IUserInteractionRepository
	{
		public UserInteractionRepository(AppDbContext context) : base(context)
		{
		}

		public void Add(UserInteraction userInteraction)
		{
			_context.UserInteractions.Add(userInteraction);
			SaveChanges();
		}

		public void Delete(int interactionId)
		{
			var interaction = _context.UserInteractions.Include(i => i.User)
									  .FirstOrDefault(i => i.InteractionId == interactionId);
			if (interaction == null)
			{
				throw new Exception($"Interaction with ID {interactionId} not found");
			}
			_context.UserInteractions.Remove(interaction);
			SaveChanges();
		}

		public List<UserInteraction> GetAll()
		{
			return _context.UserInteractions.ToList();
		}

		public UserInteraction GetByUser(int userId)
		{
			var interaction = _context.UserInteractions.Include(i => i.User)
													   .FirstOrDefault(i => i.User.UserId == userId);
			if (interaction == null)
			{
				throw new Exception($"Interaction with user ID {userId} not found");
			}	
			return interaction;
		}
	}
}
