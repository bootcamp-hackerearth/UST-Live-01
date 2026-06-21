using HealthCareApp.AdminBlazor.Dtos.Appointments;

namespace HealthCareApp.AdminBlazor.Services.Interfaces
{
    public interface IAppointmentAdminService
    {
        Task<List<AppointmentDto>> GetAllAppointmentsAsync();

        Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId);

        Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto request);

        Task<AppointmentDto?> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto request);

        Task<bool> DeleteAppointmentAsync(int appointmentId);
    }
}