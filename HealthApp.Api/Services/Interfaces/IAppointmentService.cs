using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<PagedResultDto<AppointmentDto>> GetAppointmentsAsync(AppointmentFilterDto filter);

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