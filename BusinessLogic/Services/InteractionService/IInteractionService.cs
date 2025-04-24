namespace SocialMediaApp.BusinessLogic.Services.InteractionService
{
	public interface IInteractionService
	{
		void HandleInteraction(int userId, int postId, bool liked, bool shared);
		void ToggleLike(int userId, int postId);
		void SharePost(int userId, int postId);
		void MarkPostAsSeen(int userId, int postId);
		public void MarkNotInterested(int userId, int postId);

	}
}
