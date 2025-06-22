using SocialMediaApp.DataAccess.Dtos.FriendshipDto;

namespace SocialMediaApp.BusinessLogic.Services.FriendshipService
{
	public interface IFriendshipService
	{
		bool AreUsersFriends(int userId, int friendId);
		FriendshipResponseDto GetFriendship(int friendshipId);
		List<FriendshipResponseDto> GetUserFriendships(int userId);
		Task<FriendshipResponseDto> CreateFriendship(FriendshipRequestDto friendshipDto);
		Task<FriendshipResponseDto> AcceptFriendRequest(int friendshipId);
		Task DeleteFriendship(int friendshipId, int initiatorId);
		List<FriendForShareDto> GetFriendsForShare(int currentUserId);
	}
}
