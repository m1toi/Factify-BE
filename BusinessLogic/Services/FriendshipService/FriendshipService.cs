using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.FriendshipDto;
using SocialMediaApp.DataAccess.Repositories.FriendshipRepository;

namespace SocialMediaApp.BusinessLogic.Services.FriendshipService
{
	public class FriendshipService : IFriendshipService
	{
		private readonly IFriendshipRepository _friendshipRepository;

		public FriendshipService(IFriendshipRepository friendshipRepository)
		{
			_friendshipRepository = friendshipRepository;
		}

		public bool AreUsersFriends(int userId, int friendId)
		{
			return _friendshipRepository.AreUsersFriends(userId, friendId);
		}

		public FriendshipResponseDto GetFriendship(int friendshipId)
		{
			var friendship = _friendshipRepository.GetFriendship(friendshipId);
			return friendship.ToFriendshipResponseDto();
		}

		public List<FriendshipResponseDto> GetUserFriendships(int userId)
		{
			var friendships = _friendshipRepository.GetUserFriendships(userId);
			return friendships.ToListFriendshipResponseDto();
		}

		public FriendshipResponseDto CreateFriendship(FriendshipRequestDto friendshipDto)
		{
			var friendshipEntity = friendshipDto.ToFriendship();
			var createdFriendship = _friendshipRepository.CreateFriendship(friendshipEntity);
			return createdFriendship.ToFriendshipResponseDto();
		}

		public void AcceptFriendRequest(int friendshipId)
		{
			var friendship = _friendshipRepository.GetFriendship(friendshipId);

			if (friendship == null)
				throw new Exception("Friendship not found.");

			friendship.IsConfirmed = true;

			_friendshipRepository.UpdateFriendship(friendship);
		}


		public void DeleteFriendship(int friendshipId)
		{
			_friendshipRepository.DeleteFriendship(friendshipId);
		}
	}
}
