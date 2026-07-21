using HealthApp.Shared.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface INotificationService
    {
        Task<NotificationDto> CreateAsync(NotificationCreateDto dto);

        Task<List<NotificationDto>> GetMyNotificationsAsync(string userId);

        Task<List<NotificationDto>> GetMyUnreadNotificationsAsync(string userId);

        Task<NotificationDto> MarkAsReadAsync(string notificationId);

        Task<int> CleanupOldReadNotificationsAsync(int olderThanDays);
    }
}