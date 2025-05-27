using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.MessageDto;
using SocialMediaApp.DataAccess.Repositories.MessageRepository;
using SocialMediaApp.DataAccess.Repositories.ConversationRepository;
using SocialMediaApp.DataAccess.Repositories.FriendshipRepository;
using Microsoft.AspNetCore.SignalR;
using SocialMediaApp.SignalR;
using SocialMediaApp.DataAccess.Dtos.ConversationDto;

namespace SocialMediaApp.BusinessLogic.Services.MessageService
{
	public class MessageService : IMessageService
	{
		private readonly IMessageRepository _messageRepository;
		private readonly IConversationRepository _conversationRepository;
		private readonly IFriendshipRepository _friendshipRepository;
		private readonly IHubContext<MessageHub> _hubContext;

		public MessageService(
			IMessageRepository messageRepository,
			IConversationRepository conversationRepository,
			IFriendshipRepository friendshipRepository,
			IHubContext<MessageHub> hubContext)
		{
			_messageRepository = messageRepository;
			_conversationRepository = conversationRepository;
			_friendshipRepository = friendshipRepository;
			_hubContext = hubContext;
		}

		public List<MessageResponseDto> GetMessagesByConversation(int conversationId, int userId)
		{
			// 1️⃣ Get conversation
			var conversation = _conversationRepository.GetConversation(conversationId);
			if (conversation == null)
				throw new Exception("Conversation not found.");

			// 2️⃣ Validate user is a participant
			if (conversation.User1Id != userId && conversation.User2Id != userId)
				throw new UnauthorizedAccessException("You are not a participant in this conversation.");

			// 3️⃣ Fetch messages
			var messages = _messageRepository.GetMessagesByConversation(conversationId);
			return messages.ToListMessageResponseDto();
		}

		public List<MessageResponseDto> GetMessagesByConversationPaged(
		int conversationId,
		int userId,
		int? beforeMessageId,
		int limit)
		{
			// 1️⃣ Validări identice cu GetMessagesByConversation:
			var conversation = _conversationRepository.GetConversation(conversationId);
			if (conversation == null)
				throw new Exception("Conversation not found.");

			if (conversation.User1Id != userId && conversation.User2Id != userId)
				throw new UnauthorizedAccessException("You are not a participant in this conversation.");

			// 2️⃣ Fetch paginat
			var messages = _messageRepository
				.GetMessagesByConversationPaged(conversationId, beforeMessageId, limit);

			// 3️⃣ Map la DTO-uri
			return messages.ToListMessageResponseDto();
		}

		public MessageResponseDto GetMessage(int messageId)
		{
			var message = _messageRepository.GetMessage(messageId);
			return message.ToMessageResponseDto();
		}

		public async Task<MessageResponseDto> SendMessage(MessageRequestDto messageDto)
		{
			// 1️ Get conversation
			var conversation = _conversationRepository.GetConversation(messageDto.ConversationId);
			if (conversation == null)
				throw new Exception("Conversation not found.");

			// 2️ Validate sender is participant
			if (conversation.User1Id != messageDto.SenderId && conversation.User2Id != messageDto.SenderId)
				throw new UnauthorizedAccessException("User is not a participant in this conversation.");

			if (string.IsNullOrWhiteSpace(messageDto.Content) && !messageDto.PostId.HasValue)
				throw new ArgumentException("Trebuie să aveți fie un mesaj text, fie un post de trimis.");


			// 3️ Validate friendship exists and confirmed
			bool areFriends = _friendshipRepository.AreUsersFriends(conversation.User1Id, conversation.User2Id);
			if (!areFriends)
				throw new UnauthorizedAccessException("Users are not friends. Cannot send message.");

			// 4️ Create & persist the message
			var messageEntity = messageDto.ToMessage();
			var createdMessage = _messageRepository.Create(messageEntity);
			var createdDto = createdMessage.ToMessageResponseDto();

			// 5️ Figure out the other participant’s userId
			var receiverId = conversation.User1Id == messageDto.SenderId
				? conversation.User2Id
				: conversation.User1Id;

			// 6️ Broadcast via SignalR
			await _hubContext
				.Clients
				.User(receiverId.ToString())
				.SendAsync("ReceiveMessage", createdDto);

			var convUpdate = new ConversationUpdatedDto
			{
				ConversationId = messageDto.ConversationId,
				LastMessage = createdDto.Content ?? string.Empty,
				LastMessageSenderId = createdDto.SenderId,
				LastMessageSentAt = createdDto.SentAt
			};

			// Trimitem la amândoi participanții (expeditor + destinatar)
			await _hubContext
				.Clients
				.Users(new[] { messageDto.SenderId.ToString(), receiverId.ToString() })
				.SendAsync("ConversationUpdated", convUpdate);

			return createdDto;
		}

		public void DeleteMessage(int messageId)
		{
			_messageRepository.Delete(messageId);
		}
	}
}
