using SocialMediaApp.DataAccess.Dtos.NotificationDto;
using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Entity.Enums;

namespace SocialMediaApp.BusinessLogic.Services.NotificationService
{
	public interface INotificationService
	{
	    void CreateFriendRequestNotification(int fromUserId, int toUserId, int friendshipId);
		void MarkNotificationAsReadByReference(int referenceId, NotificationType type);
		List<NotificationDto> GetUserNotifications(int userId);
		void MarkNotificationAsRead(int notificationId);
	}
}
