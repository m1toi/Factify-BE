namespace SocialMediaApp.DataAccess.Entity
{
	public class User
	{
		public int UserId { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string ProfilePicture { get; set; }
		public int RoleId { get; set; }
		public Role Role { get; set; }
		public List<Post> Posts { get; set; }
		public List<UserSeenPost> SeenPosts { get; set; }
		public List<UserInteraction> Interactions { get; set; }
		public List<UserCategoryPreference> Preferences { get; set; }
		public List<Friendship> FriendshipsInitiated { get; set; }   // Friendships where this user is UserId
		public List<Friendship> FriendshipsReceived { get; set; }    // Friendships where this user is FriendId
		public List<Conversation> ConversationsAsUser1 { get; set; } // Conversations where this user is User1
		public List<Conversation> ConversationsAsUser2 { get; set; } // Conversations where this user is User2
		public List<Message> SentMessages { get; set; }              // Messages sent by this user
	}
}
