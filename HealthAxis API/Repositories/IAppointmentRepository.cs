using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default);

        Task<List<Appointment>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default);

        Task<bool> IsSlotBookedAsync(
            int doctorId,
            DateTime scheduledDate,
            string timeSlot,
            CancellationToken ct = default);
    }
}
