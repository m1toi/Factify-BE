using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.MessageRepository
{
	public class MessageRepository : BaseRepository, IMessageRepository
	{
		public MessageRepository(AppDbContext context) : base(context) { }

		public List<Message> GetMessagesByConversation(int conversationId)
		{
			return _context.Messages
				.Where(m => m.ConversationId == conversationId)
				.Include(m => m.Sender)
				.Include(m => m.Post)
					.ThenInclude(p => p.User)
				.Include(m => m.Post)
					.ThenInclude(p => p.Category)
				.OrderBy(m => m.SentAt)
				.ToList();
		}

		public List<Message> GetMessagesByConversationPaged(int conversationId, int? beforeMessageId, int limit)
		{
			// 1) Pornim de la mesajele din conversație
			var query = _context.Messages
				.Where(m => m.ConversationId == conversationId);

			// 2) Dacă ni s-a trimis un beforeMessageId, filtrăm mesajele mai vechi
			if (beforeMessageId.HasValue)
				query = query.Where(m => m.MessageId < beforeMessageId.Value);

			// 3) Luăm „limit” mesaje, ordonate descrescător (cele mai recente primele),
			//    apoi le reorderăm crescător ca să le afișăm cronologic.
			var batch = query
				.Include(m => m.Sender)
				.Include(m => m.Post).ThenInclude(p => p.User)
				.Include(m => m.Post).ThenInclude(p => p.Category)
				.OrderByDescending(m => m.MessageId)
				.Take(limit)
				.OrderBy(m => m.MessageId)
				.ToList();

			return batch;
		}

		public Message GetMessage(int messageId)
		{
			var message = _context.Messages	
				.Include(m => m.Sender)
				.Include(m => m.Post)
				.FirstOrDefault(m => m.MessageId == messageId);

			if (message == null)
				throw new Exception($"Message with ID {messageId} not found");

			return message;
		}

		public Message Create(Message message)
		{
			if (!_context.Conversations.Any(c => c.ConversationId == message.ConversationId))
				throw new ArgumentException("Invalid Conversation ID.");

			if (!_context.Users.Any(u => u.UserId == message.SenderId))
				throw new ArgumentException("Invalid Sender ID.");

			if (message.PostId.HasValue && !_context.Posts.Any(p => p.PostId == message.PostId.Value))
				throw new ArgumentException("Invalid Post ID.");

			message.SentAt = DateTime.UtcNow;

			_context.Messages.Add(message);
			_context.SaveChanges();

			return _context.Messages
			  .Include(m => m.Sender)
			  .Include(m => m.Post)
				.ThenInclude(p => p.User)
			  .Include(m => m.Post)
				.ThenInclude(p => p.Category)
			  .Single(m => m.MessageId == message.MessageId);
		}

		public void Delete(int messageId)
		{
			var message = _context.Messages.Find(messageId);
			if (message == null)
				throw new Exception($"Message with ID {messageId} not found");

			_context.Messages.Remove(message);
			_context.SaveChanges();
		}
	}
}
