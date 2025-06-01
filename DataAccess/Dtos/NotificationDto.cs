using SocialMediaApp.DataAccess.Entity.Enums;

namespace SocialMediaApp.DataAccess.Dtos.NotificationDto
{
	public class NotificationDto
	{
		public int NotificationId { get; set; }
		public int FromUserId { get; set; }
		public string FromUsername { get; set; }

		public int ToUserId { get; set; }
		public NotificationType Type { get; set; }
		public string Message { get; set; }
		public int? ReferenceId { get; set; }
		public string? FromUserProfilePicture { get; set; }

		public bool IsRead { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
