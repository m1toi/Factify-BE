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
			// Check if a friendship already exists (in any direction)
			var existingFriendship = _context.Friendships.FirstOrDefault(f =>
				(f.UserId == friendship.UserId && f.FriendId == friendship.FriendId) ||
				(f.UserId == friendship.FriendId && f.FriendId == friendship.UserId));

			if (existingFriendship != null)
			{
				if (existingFriendship.IsConfirmed)
				{
					// Already friends → cannot send request again
					throw new Exception("A confirmed friendship already exists between these users.");
				}

				// Check if reverse pending request exists
				if (existingFriendship.UserId == friendship.FriendId && existingFriendship.FriendId == friendship.UserId)
				{
					// Auto-accept the reverse pending request
					existingFriendship.IsConfirmed = true;
					_context.Friendships.Update(existingFriendship);
					_context.SaveChanges();
					return existingFriendship;
				}

				// If a pending request already exists in same direction → block duplicate request
				throw new Exception("A friend request has already been sent to this user and is pending.");
			}

			// No friendship exists → create new pending request
			_context.Friendships.Add(friendship);
			_context.SaveChanges();
			return friendship;
		}

		public void UpdateFriendship(Friendship friendship)
		{
			_context.Friendships.Update(friendship);
			_context.SaveChanges();
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
