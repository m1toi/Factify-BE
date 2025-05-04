using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.FriendshipRepository
{
	public class FriendshipRepository : BaseRepository, IFriendshipRepository
	{
		public FriendshipRepository(AppDbContext context) : base(context) { }

		public bool AreUsersFriends(int userId, int friendId)
		{
			return _context.Friendships.Any(f =>
				(f.UserId == userId && f.FriendId == friendId && f.IsConfirmed) ||
				(f.UserId == friendId && f.FriendId == userId && f.IsConfirmed));
		}

		public Friendship GetFriendship(int friendshipId)
		{
			var friendship = _context.Friendships.FirstOrDefault(f => f.FriendshipId == friendshipId);
			if (friendship == null)
				throw new Exception($"Friendship with ID {friendshipId} not found");
			return friendship;
		}

		public List<Friendship> GetUserFriendships(int userId)
		{
			return _context.Friendships
				.Where(f => f.UserId == userId || f.FriendId == userId)
				.ToList();
		}

		public Friendship CreateFriendship(Friendship friendship)
		{
			if (AreUsersFriends(friendship.UserId, friendship.FriendId))
				throw new Exception("Friendship already exists.");

			_context.Friendships.Add(friendship);
			_context.SaveChanges();
			return friendship;
		}

		public void DeleteFriendship(int friendshipId)
		{
			var friendship = _context.Friendships.Find(friendshipId);
			if (friendship == null)
				throw new Exception($"Friendship with ID {friendshipId} not found");

			_context.Friendships.Remove(friendship);
			_context.SaveChanges();
		}
	}
}
