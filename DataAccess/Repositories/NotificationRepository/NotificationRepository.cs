using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.NotificationRepository
{
	public class NotificationRepository : BaseRepository, INotificationRepository
	{
		public NotificationRepository(AppDbContext context) : base(context) { }

		public void CreateNotification(Notification notification)
		{
			_context.Notifications.Add(notification);
			_context.SaveChanges();
		}

		public List<Notification> GetUserNotifications(int userId)
		{
			return _context.Notifications
				.Where(n => n.ToUserId == userId && !n.IsRead)    
				.Include(n => n.FromUser)   
				.OrderByDescending(n => n.CreatedAt)
				.ToList();
		}

		public void MarkAsRead(int notificationId)
		{
			var notif = _context.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
			if (notif != null)
			{
				notif.IsRead = true;
				_context.SaveChanges();
			}
		}

		public Notification GetNotificationById(int notificationId)
		{
			return _context.Notifications
				.Include(n => n.FromUser)   // IMPORTANT: includem relația FromUser
				.FirstOrDefault(n => n.NotificationId == notificationId);
		}
	}
}
