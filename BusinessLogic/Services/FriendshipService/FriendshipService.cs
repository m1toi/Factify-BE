using Microsoft.AspNetCore.SignalR;
using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.FriendshipDto;
using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Repositories.ConversationRepository;
using SocialMediaApp.DataAccess.Repositories.FriendshipRepository;
using SocialMediaApp.SignalR;

namespace SocialMediaApp.BusinessLogic.Services.FriendshipService
{
	public class FriendshipService : IFriendshipService
	{
		private readonly IFriendshipRepository _friendshipRepository;
		private readonly IConversationRepository _conversationRepository;
		private readonly IHubContext<FriendshipHub> _hubContext;

		public FriendshipService(IFriendshipRepository friendshipRepository,
			IHubContext<FriendshipHub> hubContext,
			IConversationRepository conversationRepository)
		{
			_friendshipRepository = friendshipRepository;
			_hubContext = hubContext;
			_conversationRepository = conversationRepository;
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

		public async Task<FriendshipResponseDto> CreateFriendship(FriendshipRequestDto friendshipDto)
		{
			var friendshipEntity = friendshipDto.ToFriendship();
			var createdFriendship = _friendshipRepository.CreateFriendship(friendshipEntity);
			var createdDto = createdFriendship.ToFriendshipResponseDto();

			await _hubContext.Clients
				.User(createdDto.FriendId.ToString())
				.SendAsync("FriendRequestReceived", createdDto);

			return createdDto;
		}

		public async Task<FriendshipResponseDto> AcceptFriendRequest(int friendshipId)
		{
			// 1️⃣ fetch
			var entity = _friendshipRepository.GetFriendship(friendshipId);
			if (entity == null)
				throw new Exception("Friendship not found.");

			if (entity.IsConfirmed)
				throw new Exception("Friendship already confirmed.");

			// 2️⃣ update state
			entity.IsConfirmed = true;
			_friendshipRepository.UpdateFriendship(entity);

			// 3️⃣ create conversation automatically
			var conversation = new Conversation
			{
				User1Id = entity.UserId,
				User2Id = entity.FriendId,
				CreatedAt = DateTime.UtcNow
			};
			_conversationRepository.Create(conversation);

			// 4️⃣ map to DTO
			var dto = entity.ToFriendshipResponseDto();

			// 5️⃣ broadcast la cel care a trimis cererea
			await _hubContext.Clients
				.User(entity.UserId.ToString())
				.SendAsync("FriendRequestAccepted", dto);

			return dto;
		}


		public void DeleteFriendship(int friendshipId)
		{
			_friendshipRepository.DeleteFriendship(friendshipId);
		}
	}
}
