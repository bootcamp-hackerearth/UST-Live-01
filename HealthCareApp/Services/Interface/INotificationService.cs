using HealthCareApp.Shared.Dtos.Notifications;

namespace HealthCareApp.Services.Interface
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetMyUnreadNotificationsForPatientAsync(
            string identityUserId);

        Task<NotificationDto> MarkNotificationAsReadForPatientAsync(
            int notificationId,
            string identityUserId);
    }
}