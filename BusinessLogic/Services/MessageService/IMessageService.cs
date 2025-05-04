using SocialMediaApp.DataAccess.Dtos.MessageDto;

namespace SocialMediaApp.BusinessLogic.Services.MessageService
{
	public interface IMessageService
	{
		List<MessageResponseDto> GetMessagesByConversation(int conversationId);
		MessageResponseDto GetMessage(int messageId);
		MessageResponseDto SendMessage(MessageRequestDto messageDto);
		void DeleteMessage(int messageId);
	}
}
