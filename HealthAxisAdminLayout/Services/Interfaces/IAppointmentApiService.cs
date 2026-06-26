using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.Enums;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IAppointmentApiService
    {
        Task<List<AppointmentResponseDTO>> GetAppointmentsAsync();

        Task<List<AppointmentResponseDTO>> GetAppointmentsByDoctorAsync(int doctorId);

        Task<PagedResponseDTO<AppointmentResponseDTO>> GetAppointmentsPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate
        );

        Task<List<AppointmentResponseDTO>> FilterAppointmentsAsync(
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate
        );

        Task<bool> ConfirmAppointmentAsync(int appointmentId);

        Task<bool> CancelAppointmentAsync(int appointmentId, string reason);

        Task<bool> DeleteAppointmentAsync(int appointmentId);
    }
}