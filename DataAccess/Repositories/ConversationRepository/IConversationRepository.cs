using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.ConversationRepository
{
	public interface IConversationRepository
	{
		Conversation GetConversation(int conversationId);
		Conversation GetConversationBetweenUsers(int user1Id, int user2Id);
		Conversation Create(Conversation conversation);
		List<Conversation> GetUserConversations(int userId);
	}
}
	