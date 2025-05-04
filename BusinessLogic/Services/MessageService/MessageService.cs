using SocialMediaApp.DataAccess.Dtos.MessageDto;
using SocialMediaApp.DataAccess.Repositories.MessageRepository;
using SocialMediaApp.BusinessLogic.Mapping;

namespace SocialMediaApp.BusinessLogic.Services.MessageService
{
	public class MessageService : IMessageService
	{
		private readonly IMessageRepository _messageRepository;

		public MessageService(IMessageRepository messageRepository)
		{
			_messageRepository = messageRepository;
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
