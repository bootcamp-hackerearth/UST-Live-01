using Healthcare.Shared.DTOs.Appointments;
using HealthCare.Api.Models;


namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<string>> AvailableTimeSlots(DateOnly date, int doctorId);
        Task<bool> IsAvailable(DateOnly date, int doctorId, string timeSlot);
        Task<List<AppointmentReportDto>> GetDailyReport();
        Task<List<AppointmentListDto>> GetDoctorSchedule(DateOnly date, int id);
        Task<List<AppointmentListDto>> GetPatientSchedule(DateOnly date, int id);
        Task<List<AppointmentListDto>> GetAppointmentByPatient(int id);
        Task<List<AppointmentListDto>> GetAppointmentByDoctor(int id);
        Task CancelAppointmentsByDoctorDate(int doctorId, DateOnly date);

    }
}
