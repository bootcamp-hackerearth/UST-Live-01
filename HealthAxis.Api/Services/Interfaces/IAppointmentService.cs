using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.DTOs;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(int id);
        Task<AppointmentDto> AddAsync(AppointmentDto entity);
        Task<AppointmentDto> UpdateAsync(int id, AppointmentDto entity);
        Task<AppointmentDto> DeleteAsync(int id);

    }
}
