using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.ConversationRepository
{
	public class ConversationRepository : BaseRepository, IConversationRepository
	{
		public ConversationRepository(AppDbContext context) : base(context) { }

		public Conversation GetConversation(int conversationId)
		{
			var conversation = _context.Conversations
				.Include(c => c.Messages)
				.Include(c => c.User1)
				.Include(c => c.User2)
				.Include(c => c.Messages)
				.FirstOrDefault(c => c.ConversationId == conversationId);

			if (conversation == null)
				throw new Exception($"Conversation with ID {conversationId} not found");

			return conversation;
		}

		public Conversation GetConversationBetweenUsers(int user1Id, int user2Id)
		{
			return _context.Conversations
				.FirstOrDefault(c =>
					(c.User1Id == user1Id && c.User2Id == user2Id) ||
					(c.User1Id == user2Id && c.User2Id == user1Id));
		}

		public Conversation Create(Conversation conversation)
		{
			if (_context.Conversations.Any(c =>
				(c.User1Id == conversation.User1Id && c.User2Id == conversation.User2Id) ||
				(c.User1Id == conversation.User2Id && c.User2Id == conversation.User1Id)))
			{
				throw new Exception("Conversation between these users already exists.");
			}
			conversation.CreatedAt = DateTimeOffset.UtcNow;

			_context.Conversations.Add(conversation);
			_context.SaveChanges();
			return conversation;
		}

		public List<Conversation> GetUserConversations(int userId)
		{
			return _context.Conversations
				.Where(c => c.User1Id == userId || c.User2Id == userId)
				.Include(c => c.User1)
				.Include(c => c.User2)
				.Include(c => c.Messages)
				.ToList();
		}
	}
}
