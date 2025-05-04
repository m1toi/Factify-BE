using SocialMediaApp.DataAccess.Dtos.FriendshipDto;

namespace SocialMediaApp.BusinessLogic.Services.FriendshipService
{
	public interface IFriendshipService
	{
		bool AreUsersFriends(int userId, int friendId);
		FriendshipResponseDto GetFriendship(int friendshipId);
		List<FriendshipResponseDto> GetUserFriendships(int userId);
		FriendshipResponseDto CreateFriendship(FriendshipRequestDto friendshipDto);
		void AcceptFriendRequest(int friendshipId);
		void DeleteFriendship(int friendshipId);
	}
}
