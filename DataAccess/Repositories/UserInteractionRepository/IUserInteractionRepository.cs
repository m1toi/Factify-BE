using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.UserInteractionRepository
{
	public interface IUserInteractionRepository
	{
		public List<UserInteraction> GetAll();
		public UserInteraction GetByUser(int userId);
		public void Add(UserInteraction userInteraction);
		public void Delete(int interactionId);
	}
}
