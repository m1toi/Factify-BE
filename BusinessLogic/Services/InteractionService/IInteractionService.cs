namespace SocialMediaApp.BusinessLogic.Services.InteractionService
{
	public interface IInteractionService
	{
		void HandleInteraction(int userId, int postId, bool liked, bool shared);
	}
}
