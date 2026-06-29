using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto entity, int patientId);
        Task<AppointmentDto?> UpdateAsync(int id, UpdateAppointmentStatusDto entity);
        Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<bool> DeleteAppointmentAsync(int appointmentId);
        Task<List<AppointmentDto>> GetAppointmentsByPatientNameAsync(string patientName);
        Task<List<AppointmentDto>> GetAppointmentsByDoctorNameAsync(string doctorName);
    }
}
