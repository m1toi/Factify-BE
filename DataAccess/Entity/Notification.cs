using SocialMediaApp.DataAccess.Entity.Enums;

namespace SocialMediaApp.DataAccess.Entity
{
	public class Notification
	{
		public int NotificationId { get; set; }

		public int FromUserId { get; set; }
		public User FromUser { get; set; }

		public int ToUserId { get; set; }
		public User ToUser { get; set; }

		public NotificationType Type { get; set; }
		public string Message { get; set; }

		public int? ReferenceId { get; set; }
		public bool IsRead { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
