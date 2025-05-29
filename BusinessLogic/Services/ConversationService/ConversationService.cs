using Microsoft.AspNetCore.SignalR;
using SocialMediaApp.SignalR;
using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.ConversationDto;
using SocialMediaApp.DataAccess.Repositories.ConversationRepository;
using SocialMediaApp.DataAccess.Repositories.MessageRepository;

namespace SocialMediaApp.BusinessLogic.Services.ConversationService
{
	public class ConversationService : IConversationService
	{
		private readonly IConversationRepository _conversationRepository;
		private readonly IMessageRepository _messageRepository;
		private readonly IHubContext<MessageHub> _hubContext;

		public ConversationService(IConversationRepository conversationRepository,IMessageRepository messageRepository, IHubContext<MessageHub> hubContext)
		{
			_conversationRepository = conversationRepository;
			_messageRepository = messageRepository;
			_hubContext = hubContext;
		}
		public List<ParticipantDto> GetConversationParticipants(int conversationId)
		{
			var conv = _conversationRepository.GetConversation(conversationId);
			return conv.ToParticipantDtos();
		}

		public ConversationResponseDto GetConversation(int conversationId, int currentUserId)
		{
			var conversation = _conversationRepository.GetConversation(conversationId);
			return conversation.ToConversationResponseDto(currentUserId);
		}

		public ConversationResponseDto GetConversationBetweenUsers(int user1Id, int user2Id, int currentUserId)
		{
			var conversation = _conversationRepository.GetConversationBetweenUsers(user1Id, user2Id);
			return conversation?.ToConversationResponseDto(currentUserId);
		}

		public ConversationResponseDto CreateConversation(ConversationRequestDto conversationDto, int currentUserId)
		{
			var conversationEntity = conversationDto.ToConversation();
			var createdConversation = _conversationRepository.Create(conversationEntity);
			return createdConversation.ToConversationResponseDto(currentUserId);
		}

		public List<ConversationResponseDto> GetUserConversations(int userId)
		{
			var convs = _conversationRepository.GetUserConversations(userId);
			// Aici apelezi noul overload care primește și userId-ul
			return convs.ToListConversationResponseDto(userId)
						.OrderByDescending(c =>
							(c.LastMessageSentAt ?? c.CreatedAt).UtcDateTime
						)
						.ToList();
		}


		public async Task MarkConversationAsReadAsync(int conversationId, int userId)
		{
			var conv = _conversationRepository.GetConversation(conversationId);
			if (conv.User1Id != userId && conv.User2Id != userId)
				throw new UnauthorizedAccessException();

			// marchează în BD
			_messageRepository.MarkMessagesAsRead(conversationId, userId);

			// recalculează necititele
			var unread = _messageRepository.CountUnread(conversationId, userId);

			// construiește DTO‐ul de read
			var readDto = new ConversationReadDto
			{
				ConversationId = conversationId,
				UnreadCount = unread,
				HasUnread = unread > 0
			};

			// trimite doar acest eveniment
			await _hubContext
			  .Clients
			  .User(userId.ToString())   // numai la cel care chiar a deschis conversația
			  .SendAsync("ConversationRead", readDto);
		}

	}
}
