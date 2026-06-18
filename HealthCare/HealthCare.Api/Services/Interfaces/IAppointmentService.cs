using HealthCare.Api.DTOs;
using HealthCare.Api.DTOs.Appointment;
using HealthCare.Api.DTOs.Appointments;

namespace HealthCare.Api.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task AddAsync(CreateAppointmentDto dto,int id);

        Task DeleteAsync(int id);
        Task UpdateAsync(int id,UpdateAppointmentDto dto);
        Task UpdateStatusAsync(int id, UpdateAppointmentDto dto);
        Task<AppointmentListDto> GetByIdAsync(int id);
        Task<PagedResult<AppointmentListDto>> GetAllAsync(AppointmentFilter filter);
        Task<bool> IsAvailable(DateOnly date, int doctorId, string timeSlot);
        Task<List<AppointmentReportDto>> GetDailyReport();
        Task<List<string>> AvailableTimeSlots(DateOnly date, int doctorId);
        Task<List<AppointmentListDto>> GetDoctorSchedule(DateOnly date, int id);
        Task<List<AppointmentListDto>> GetPatientSchedule(DateOnly date, int id);
        Task<List<AppointmentListDto>> GetAppointmentByPatient(int id);
        Task<List<AppointmentListDto>> GetAppointmentByDoctor(int id);
        Task CancelAppointmentsByDoctorDate(int doctorId, DateOnly date);
    }
}
