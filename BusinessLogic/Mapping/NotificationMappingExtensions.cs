using SocialMediaApp.DataAccess.Dtos.NotificationDto;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.BusinessLogic.Mapping
{
	public static class NotificationMappingExtensions
	{
		public static NotificationDto ToNotificationDto(this Notification notification)
		{
			return new NotificationDto
			{
				NotificationId = notification.NotificationId,
				FromUserId = notification.FromUserId,
				FromUsername = notification.FromUser?.Username,
				ToUserId = notification.ToUserId,
				Type = notification.Type,
				Message = notification.Message,
				ReferenceId = notification.ReferenceId,
				IsRead = notification.IsRead,
				CreatedAt = notification.CreatedAt
			};
		}

		public static List<NotificationDto> ToNotificationDtoList(this List<Notification> notifications)
		{
			return notifications.Select(n => n.ToNotificationDto()).ToList();
		}
	}
}
