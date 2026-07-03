using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.Enums;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IAppointmentApiService
    {
        Task<List<AppointmentResponseDto>> GetAppointmentsAsync();

        Task<List<AppointmentResponseDto>> GetAppointmentsByDoctorAsync(int doctorId);

        Task<PagedResponseDto<AppointmentResponseDto>> GetAppointmentsPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate
        );

        Task<List<AppointmentResponseDto>> FilterAppointmentsAsync(
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate
        );

        Task<bool> ConfirmAppointmentAsync(int appointmentId);

        Task<bool> CancelAppointmentAsync(int appointmentId, string reason);

        Task<bool> DeleteAppointmentAsync(int appointmentId);
    }
}