using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(
            CreateNotificationDto dto,
            CancellationToken ct = default);

        Task<IReadOnlyList<NotificationDto>>
            GetUnreadDoctorLeaveNotificationsAsync(
                string recipientUserId,
                CancellationToken ct = default);

        Task AcknowledgeNotificationAsync(
            int notificationId,
            string recipientUserId,
            CancellationToken ct = default);
    }
}
