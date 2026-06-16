using HealthAxisApplicn.Dto;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto?>> GetAllAsync();
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<AppointmentDto> CreateAsync(AppointmentDto entity);
        Task<AppointmentDto?> UpdatebyAsync(int id, AppointmentDto entity);
        Task<List<AppointmentDto>> GetAppointmentByPatientIdAsync(int patientId);
        Task<List<AppointmentDto>> GetAppointmentByDoctorIdAsync(int doctorId);
        Task<AppointmentDto?> DeleteAppointmentAsync(int appointmentId);
    }
}
