using HealthAxis.API.Dtos.AppointmentDtos;

namespace HealthAxis.API.Services.Interfaces;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAllAppointmentsAsync();

    Task<AppointmentDto?> GetAppointmentByIdAsync(int id);

    Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto dto);

    Task<bool> DeleteAppointmentAsync(int id);
}