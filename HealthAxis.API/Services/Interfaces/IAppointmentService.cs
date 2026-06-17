using HealthAxis.API.DTO;
using HealthAxis.DTO.AppointmentDto;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();

        Task<AppointmentDto?> GetByIdAsync(int id);

        Task<AppointmentDto> AddAsync(CreateAppointmentDto appointmentDto);

        //Task<AppointmentDto?> UpdateAsync(int id, AppointmentDto appointmentDto);
        Task<AppointmentDto> UpdateStatusAsync(int id, UpdateAppointmentStatusDto statusDto);

        Task<AppointmentDto?> DeleteAsync(int id);
    }
}

