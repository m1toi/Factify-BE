using SocialMediaApp.DataAccess.Dtos.MessageDto;

namespace SocialMediaApp.BusinessLogic.Services.MessageService
{
	public interface IMessageService
	{
		public List<MessageResponseDto> GetMessagesByConversation(int conversationId, int userId);
		MessageResponseDto GetMessage(int messageId);
		MessageResponseDto SendMessage(MessageRequestDto messageDto);
		void DeleteMessage(int messageId);
	}
}
