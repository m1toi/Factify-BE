using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.MessageRepository
{
	public interface IMessageRepository
	{
		List<Message> GetMessagesByConversation(int conversationId);
		List<Message> GetMessagesByConversationPaged(int conversationId, int? beforeMessageId, int limit);
		Message GetMessage(int messageId);
		Message Create(Message message);
		void Delete(int messageId);
	}
}
