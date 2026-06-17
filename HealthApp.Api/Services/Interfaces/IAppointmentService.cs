using HealthApp.Api.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
        public interface IAppointmentService
        {
            Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
            Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
            Task<AppointmentDto> CreateAppointmentAsync(AppointmentCreateDto dto);
        }
}
