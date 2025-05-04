using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Dtos.FriendshipDto;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaApp.BusinessLogic.Mapping
{
	public static class FriendshipMappingExtensions
	{
		public static Friendship ToFriendship(this FriendshipRequestDto dto)
		{
			return new Friendship
			{
				UserId = dto.UserId,
				FriendId = dto.FriendId,
				IsConfirmed = dto.IsConfirmed,
				CreatedAt = DateTime.UtcNow
			};
		}

		public static FriendshipResponseDto ToFriendshipResponseDto(this Friendship friendship)
		{
			return new FriendshipResponseDto
			{
				FriendshipId = friendship.FriendshipId,
				UserId = friendship.UserId,
				Username = friendship.User?.Username,
				FriendId = friendship.FriendId,
				FriendUsername = friendship.Friend?.Username,
				CreatedAt = friendship.CreatedAt,
				IsConfirmed = friendship.IsConfirmed
			};
		}

		public static List<FriendshipResponseDto> ToListFriendshipResponseDto(this List<Friendship> friendships)
		{
			return friendships.Select(f => f.ToFriendshipResponseDto()).ToList();
		}
	}
}
