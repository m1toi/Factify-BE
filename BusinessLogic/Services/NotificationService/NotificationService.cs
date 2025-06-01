using Microsoft.AspNetCore.SignalR;
using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.NotificationDto;
using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Entity.Enums;
using SocialMediaApp.DataAccess.Repositories.NotificationRepository;
using SocialMediaApp.SignalR;

namespace SocialMediaApp.BusinessLogic.Services.NotificationService
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationRepository _notificationRepo;
		private readonly IHubContext<NotificationHub> _hubContext;

		public NotificationService(
			INotificationRepository notificationRepo,
			IHubContext<NotificationHub> hubContext
		)
		{
			_notificationRepo = notificationRepo;
			_hubContext = hubContext;
		}


		public void CreateFriendRequestNotification(int fromUserId, int toUserId, int friendshipId)
		{
			var notification = new Notification
			{
				FromUserId = fromUserId,
				ToUserId = toUserId,
				Type = NotificationType.FriendRequest,
				Message = "sent you a friend request.",
				IsRead = false,
				ReferenceId = friendshipId,
				CreatedAt = DateTime.UtcNow
			};

			_notificationRepo.CreateNotification(notification);

			var dto = notification.ToNotificationDto();

			_hubContext.Clients.User(toUserId.ToString())
				.SendAsync("ReceiveNotification", dto);
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
