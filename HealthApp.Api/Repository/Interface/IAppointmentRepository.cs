using HealthApp.Api.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {

        Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot);

        Task<List<Appointment>?> GetUpcomingByDoctorAsync(int doctorId, DateTime from, DateTime to);

        Task<List<string>?> GetBookedSlotsAsync(int doctorId, DateTime date);

        Task<Appointment?> UpdateStatusAsync(int appointmentId,string status);

        Task<Appointment?> CancelAppointmentAsync(int appointmentId, string reason);

        Task<List<Appointment>?> GetByPatientAndDoctor(int? patientId,int? doctorId);
    }
}
