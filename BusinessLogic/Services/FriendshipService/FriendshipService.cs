using Microsoft.AspNetCore.SignalR;
using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.BusinessLogic.Services.NotificationService;
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
		private readonly INotificationService _notificationService;

		public FriendshipService(IFriendshipRepository friendshipRepository,
			IHubContext<FriendshipHub> hubContext,
			IConversationRepository conversationRepository,
			INotificationService notificationService)
		{
			_friendshipRepository = friendshipRepository;
			_hubContext = hubContext;
			_conversationRepository = conversationRepository;
			_notificationService = notificationService;
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

			_notificationService.CreateFriendRequestNotification(
				friendshipDto.UserId,
				friendshipDto.FriendId,
				createdFriendship.FriendshipId
			);

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
				return entity.ToFriendshipResponseDto();

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

		public List<FriendForShareDto> GetFriendsForShare(int currentUserId)
		{
			// 1️⃣ Preia toate conversațiile userului
			var conversations = _conversationRepository.GetUserConversations(currentUserId);

			// 2️⃣ Obține userId-urile prietenilor confirmați
			var confirmedFriends = _friendshipRepository
				.GetUserFriendships(currentUserId)
				.Where(f => f.IsConfirmed)
				.Select(f =>
					f.UserId == currentUserId
						? f.FriendId
						: f.UserId
				)
				.ToHashSet();

			// 3️⃣ Filtrăm conversațiile care includ doar prieteni confirmați
			var result = conversations
				.Where(conv =>
				{
					var otherUserId = conv.User1Id == currentUserId
						? conv.User2Id
						: conv.User1Id;
					return confirmedFriends.Contains(otherUserId);
				})
				.Select(conv =>
				{
					var friendUser = conv.User1Id == currentUserId
						? conv.User2
						: conv.User1;

					var lastChat = conv.Messages != null && conv.Messages.Any()
						? conv.Messages.Max(m => m.SentAt)
						: conv.CreatedAt;

					return new FriendForShareDto
					{
						UserId = friendUser.UserId,
						Username = friendUser.Username,
						ProfilePicture = friendUser.ProfilePicture,
						LastChatAt = lastChat
					};
				})
				.OrderByDescending(f => f.LastChatAt)
				.ToList();

			return result;
		}

	}
}
