using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.FriendshipRepository
{
	public interface IFriendshipRepository
	{
		bool AreUsersFriends(int userId, int friendId);
		Friendship GetFriendship(int friendshipId);
		List<Friendship> GetUserFriendships(int userId);
		Friendship CreateFriendship(Friendship friendship);
		void DeleteFriendship(int friendshipId);
	}
}
