using SocialMediaApp.DataAccess.Dtos.MessageDto;

namespace SocialMediaApp.BusinessLogic.Services.MessageService
{
	public interface IMessageService
	{
		public List<MessageResponseDto> GetMessagesByConversation(int conversationId, int userId);
		List<MessageResponseDto> GetMessagesByConversationPaged(int conversationId, int userId, int? beforeMessageId, int limit);
		MessageResponseDto GetMessage(int messageId);
	    Task<MessageResponseDto> SendMessage(MessageRequestDto messageDto);
		void DeleteMessage(int messageId);
	}
}
