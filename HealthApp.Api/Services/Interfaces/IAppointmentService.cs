using HealthApp.Api.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
        public interface IAppointmentService
        {
            Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(CancellationToken ct = default);
            Task<AppointmentDto?> GetAppointmentByIdAsync(int id, CancellationToken ct = default);
            Task<AppointmentDto> CreateAppointmentAsync(AppointmentCreateDto dto, CancellationToken ct = default);
        }
}
