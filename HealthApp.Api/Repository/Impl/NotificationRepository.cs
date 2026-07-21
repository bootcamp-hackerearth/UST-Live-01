using HealthApp.Api.Data;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repository.Impl
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly HealthAppDbContext _context;

        public NotificationRepository(HealthAppDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            return notification;
        }

        public async Task<List<Notification>> GetByUserIdAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetUnreadByUserIdAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(string notificationId)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<Notification?> MarkAsReadAsync(string notificationId)
        {
            var notification = await GetByIdAsync(notificationId);

            if (notification == null)
                return null;

            notification.IsRead = true;

            await _context.SaveChangesAsync();

            return notification;
        }


        public async Task<int> DeleteReadNotificationsOlderThanAsync(DateTime cutoffDate)
        {
            var oldReadNotifications = await _context.Notifications
                .Where(n => n.IsRead && n.CreatedAt < cutoffDate)
                .ToListAsync();

            if (!oldReadNotifications.Any())
                return 0;

            _context.Notifications.RemoveRange(oldReadNotifications);

            await _context.SaveChangesAsync();

            return oldReadNotifications.Count;
        }
    }
}