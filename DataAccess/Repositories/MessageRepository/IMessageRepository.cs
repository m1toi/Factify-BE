using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.MessageRepository
{
	public interface IMessageRepository
	{
		List<Message> GetMessagesByConversation(int conversationId);
		Message GetMessage(int messageId);
		Message Create(Message message);
		void Delete(int messageId);
	}
}
