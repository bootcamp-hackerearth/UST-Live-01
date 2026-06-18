using HealthAxis.API.DTOs;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAsync();

        Task<AppointmentDto> AddAsync(CreateAppointmentDto dto);

        Task<AppointmentDto> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto);

        Task<bool> DeleteAsync(int id);
    }
}