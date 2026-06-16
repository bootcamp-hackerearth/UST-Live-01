using HealthCare.Api.DTOs.Appointments;

namespace HealthCare.Api.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task AddAsync(CreateAppointmentDto dto);

        Task DeleteAsync(int id);
        Task UpdateAsync(int id,UpdateAppointmentDto dto);

        Task<AppointmentListDto> GetByIdAsync(int id);

        Task<IEnumerable<AppointmentListDto>> GetAllAsync();
    }
}
