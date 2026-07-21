using HealthApp.Api.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface INotificationRepository
    {
        Task<Notification> AddAsync(Notification notification);

        Task<List<Notification>> GetByUserIdAsync(string userId);

        Task<List<Notification>> GetUnreadByUserIdAsync(string userId);

        Task<Notification?> GetByIdAsync(string notificationId);

        Task<Notification?> MarkAsReadAsync(string notificationId);

        Task<int> DeleteReadNotificationsOlderThanAsync(DateTime cutoffDate);
    }
}