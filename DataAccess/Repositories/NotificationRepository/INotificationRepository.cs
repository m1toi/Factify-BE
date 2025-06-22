using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Entity.Enums;

namespace SocialMediaApp.DataAccess.Repositories.NotificationRepository
{
	public interface INotificationRepository
	{
		void CreateNotification(Notification notification);
		List<Notification> GetUserNotifications(int userId);
		void MarkAsRead(int notificationId);
		Notification GetNotificationById(int notificationId);
		void MarkAsReadByReference(int referenceId, NotificationType type);
	}
}
