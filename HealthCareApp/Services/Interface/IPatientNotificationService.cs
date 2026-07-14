using HealthCareApp.Shared.Dtos.PatientNotifications;

namespace HealthCareApp.Services.Interface
{
    public interface IPatientNotificationService
    {
        Task<List<PatientNotificationDto>> GetMyUnreadNotificationsAsync(
            string identityUserId);

        Task<PatientNotificationDto> MarkAsReadAsync(
            int patientNotificationId,
            string identityUserId);

        Task CreateNotificationAsync(
            int patientId,
            int? doctorId,
            int? appointmentId,
            string title,
            string message,
            string notificationType);
    }
}