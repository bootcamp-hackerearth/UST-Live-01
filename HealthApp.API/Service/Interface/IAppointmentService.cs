using HealthApp.API.Models.DTOs;

namespace HealthApp.API.Service.Interface
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(int id);
        Task<AppointmentDto> AddAsync(AppointmentDto entity);
        Task<AppointmentDto> UpdateAsync(int id, AppointmentDto entity);
    }
}
