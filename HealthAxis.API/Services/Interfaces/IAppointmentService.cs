using HealthAxis.Shared.DTO.AppointmentDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();

        Task<AppointmentDto?> GetByIdAsync(int id);

        Task<List<AppointmentDto>> GetByPatientIdAsync(int patientId);

        Task<AppointmentDto> AddAsync(CreateAppointmentDto appointmentDto);

        Task<AppointmentDto> UpdateStatusAsync(
            int id,
            UpdateAppointmentStatusDto statusDto);

        Task<AppointmentDto?> DeleteAsync(int id);
    }
}