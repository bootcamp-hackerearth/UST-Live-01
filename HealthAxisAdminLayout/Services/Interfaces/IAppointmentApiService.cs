using HealthAxisAdminLayout.DTOs.Appointment;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IAppointmentApiService
    {
        Task<List<AppointmentResponseDTO>> GetAppointmentsAsync();

        Task<bool> ConfirmAppointmentAsync(int appointmentId);

        Task<bool> DeleteAppointmentAsync(int appointmentId);
    }
}