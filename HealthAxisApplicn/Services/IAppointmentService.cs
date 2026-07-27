using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync(int page,int pageSize);
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto entity, int patientId);
        Task<AppointmentDto?> UpdateAsync(int id, UpdateAppointmentStatusDto entity, string role);
        Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId, int page, int pageSize);
        Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId,int page,int pageSize);
        Task<bool> DeleteAppointmentAsync(int appointmentId);
        Task<List<AppointmentDto>> GetAppointmentsByPatientNameAsync(string patientName);
        Task<List<AppointmentDto>> GetAppointmentsByDoctorNameAsync(string doctorName);
        Task<List<AppointmentDto>> GetTodayAppointmentsAsync(int doctorId, int page, int pageSize);
    }
}
