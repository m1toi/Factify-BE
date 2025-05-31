using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.NotificationRepository
{
	public interface INotificationRepository
	{
		void CreateNotification(Notification notification);
		List<Notification> GetUserNotifications(int userId);
		void MarkAsRead(int notificationId);
	}
}
