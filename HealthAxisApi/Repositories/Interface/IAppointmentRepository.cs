using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Enums;

namespace HealthAxisCore_Api.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetByDoctor(int doctorId);

        Task<IEnumerable<Appointment>> GetByPatient(int patientId);

        Task<IEnumerable<Appointment>> FilterAppointments(AppointmentStatus? status, DateTime? startDate, DateTime? endDate);

        Task CancelAppointment(int id, string reason);

        Task ConfirmAppointment(int id);
    }
}