using HealthApp.Api.Model;
using HospitalManagementAPI.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<Appointment?> GetDoctorByIdAsync(int doctorId);

        Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot);

        Task<List<Appointment>?> GetByPatientAsync(int patientId);

        Task<List<Appointment>?> GetUpcomingByDoctorAsync(int doctorId, DateTime from, DateTime to);

        Task<List<string>?> GetBookedSlotsAsync(int doctorId, DateTime date);

        Task<Appointment?> UpdateStatusAsync(int appointmentId,string status);

        Task<Appointment?> CancelAppointmentAsync(int appointmentId, string reason);
    }
}
