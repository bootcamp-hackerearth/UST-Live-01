using HealthAxis.API.Events;
using HealthAxis.API.Models;
using HealthAxis.Shared.DTO.NotificationDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool>
            CreateAppointmentBookedNotificationAsync(
                AppointmentBookedEvent appointmentBookedEvent,
                CancellationToken cancellationToken = default);

        Task<int>
            CreateAppointmentStatusNotificationsAsync(
                Appointment appointment,
                CancellationToken cancellationToken = default);

        Task<List<NotificationDto>>
            GetPatientNotificationsAsync(
                int patientId,
                CancellationToken cancellationToken = default);

        Task<List<NotificationDto>>
            GetDoctorNotificationsAsync(
                int doctorId,
                CancellationToken cancellationToken = default);

        Task<int> GetPatientUnreadCountAsync(
            int patientId,
            CancellationToken cancellationToken = default);

        Task<int> GetDoctorUnreadCountAsync(
            int doctorId,
            CancellationToken cancellationToken = default);

        Task MarkPatientNotificationAsReadAsync(
            int notificationId,
            int patientId,
            CancellationToken cancellationToken = default);

        Task MarkDoctorNotificationAsReadAsync(
            int notificationId,
            int doctorId,
            CancellationToken cancellationToken = default);

        Task<int>
            MarkAllPatientNotificationsAsReadAsync(
                int patientId,
                CancellationToken cancellationToken = default);

        Task<int>
            MarkAllDoctorNotificationsAsReadAsync(
                int doctorId,
                CancellationToken cancellationToken = default);
    }
}