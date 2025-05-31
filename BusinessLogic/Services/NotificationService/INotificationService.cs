using SocialMediaApp.DataAccess.Dtos.NotificationDto;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.BusinessLogic.Services.NotificationService
{
	public interface INotificationService
	{
	    void CreateFriendRequestNotification(int fromUserId, int toUserId, int friendshipId);
		List<NotificationDto> GetUserNotifications(int userId);
		void MarkNotificationAsRead(int notificationId);
	}
}
