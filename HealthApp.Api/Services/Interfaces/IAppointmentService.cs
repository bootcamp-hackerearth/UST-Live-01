using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAppointmentsAsync(
            int? doctorId = null,
            int? patientId = null,
            bool onlyUpcoming = false);

        Task<AppointmentDto> GetAppointmentByIdAsync(int id);

        Task<AppointmentDto> BookAppointmentAsync(AppointmentCreateDto dto);

        Task UpdateAppointmentStatusAsync(
            int id,
            AppointmentStatus status,
            string? cancellationReason = null);

        Task<IEnumerable<string>> GetAvailableSlotsAsync(
            int doctorId,
            DateOnly date);

        Task DeleteAppointmentAsync(int id);
    }
}