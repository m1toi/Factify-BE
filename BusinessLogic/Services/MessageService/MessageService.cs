using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.MessageDto;
using SocialMediaApp.DataAccess.Repositories.MessageRepository;
using SocialMediaApp.DataAccess.Repositories.ConversationRepository;
using SocialMediaApp.DataAccess.Repositories.FriendshipRepository;

namespace SocialMediaApp.BusinessLogic.Services.MessageService
{
	public class MessageService : IMessageService
	{
		private readonly IMessageRepository _messageRepository;
		private readonly IConversationRepository _conversationRepository;
		private readonly IFriendshipRepository _friendshipRepository;

		public MessageService(
			IMessageRepository messageRepository,
			IConversationRepository conversationRepository,
			IFriendshipRepository friendshipRepository)
		{
			_messageRepository = messageRepository;
			_conversationRepository = conversationRepository;
			_friendshipRepository = friendshipRepository;
		}

		public List<MessageResponseDto> GetMessagesByConversation(int conversationId)
		{
			var messages = _messageRepository.GetMessagesByConversation(conversationId);
			return messages.ToListMessageResponseDto();
		}

		public MessageResponseDto GetMessage(int messageId)
		{
			var message = _messageRepository.GetMessage(messageId);
			return message.ToMessageResponseDto();
		}

		public MessageResponseDto SendMessage(MessageRequestDto messageDto)
		{
			// 1️⃣ Get conversation
			var conversation = _conversationRepository.GetConversation(messageDto.ConversationId);
			if (conversation == null)
				throw new Exception("Conversation not found.");

			// 2️⃣ Validate sender is participant
			if (conversation.User1Id != messageDto.SenderId && conversation.User2Id != messageDto.SenderId)
				throw new UnauthorizedAccessException("User is not a participant in this conversation.");

			// 3️⃣ Validate friendship exists and confirmed
			bool areFriends = _friendshipRepository.AreUsersFriends(conversation.User1Id, conversation.User2Id);
			if (!areFriends)
				throw new UnauthorizedAccessException("Users are not friends. Cannot send message.");

			// 4️⃣ Proceed to create message
			var messageEntity = messageDto.ToMessage();
			var createdMessage = _messageRepository.Create(messageEntity);
			return createdMessage.ToMessageResponseDto();
		}

		public void DeleteMessage(int messageId)
		{
			_messageRepository.Delete(messageId);
		}
	}
}
