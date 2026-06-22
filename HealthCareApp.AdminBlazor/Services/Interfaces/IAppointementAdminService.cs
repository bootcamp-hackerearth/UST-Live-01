using HealthCareApp.Shared.Dtos.Appointments;
namespace HealthCareApp.AdminBlazor.Services.Interfaces
{
    public interface IAppointmentAdminService
    {
        Task<List<AppointmentDto>> GetAllAppointmentsAsync();

        Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId);

        Task<AppointmentDto> CreateAppointmentAsync(BookAppointmentDto request);

        Task<AppointmentDto?> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto request);

        Task<bool> DeleteAppointmentAsync(int appointmentId);
    }
}
