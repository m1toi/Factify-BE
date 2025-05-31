using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.NotificationDto;
using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Entity.Enums;
using SocialMediaApp.DataAccess.Repositories.NotificationRepository;

namespace SocialMediaApp.BusinessLogic.Services.NotificationService
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationRepository _notificationRepo;

		public NotificationService(INotificationRepository notificationRepo)
		{
			_notificationRepo = notificationRepo;
		}

		public void CreateFriendRequestNotification(int fromUserId, int toUserId, int friendshipId)
		{
			var notification = new Notification
			{
				FromUserId = fromUserId,
				ToUserId = toUserId,
				Type = NotificationType.FriendRequest,
				Message = "You received a friend request.",
				IsRead = false,
				ReferenceId = friendshipId,
				CreatedAt = DateTime.UtcNow
			};

			_notificationRepo.CreateNotification(notification);
		}


		public List<NotificationDto> GetUserNotifications(int userId)
		{
			var list = _notificationRepo.GetUserNotifications(userId);
			return list.ToNotificationDtoList();
		}

		public void MarkNotificationAsRead(int notificationId)
		{
			_notificationRepo.MarkAsRead(notificationId);
		}
	}
}
