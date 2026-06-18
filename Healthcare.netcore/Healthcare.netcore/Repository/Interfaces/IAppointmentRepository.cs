using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {

        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);

        Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId);

        Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot);

    }
}