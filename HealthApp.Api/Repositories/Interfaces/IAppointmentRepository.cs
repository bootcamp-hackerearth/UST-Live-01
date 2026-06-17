using HealthApp.Api.Models;

namespace HealthApp.Api.Repositories.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<bool> IsDoctorSlotBookedAsync(
            int doctorId,
            DateOnly date,
            string slot,
            CancellationToken ct = default);

        Task<bool> HasPatientSlotConflictAsync(
            int patientId,
            DateOnly date,
            string slot,
            CancellationToken ct = default);
            

        Task<bool> HasAppointmentWithDoctorOnSameDayAsync(
            int patientId,
            int doctorId,
            DateOnly date,
            CancellationToken ct = default
            );

        Task<IEnumerable<Appointment>> GetAppointmentsAsync(
            int? doctorId = null,
            int? patientId = null,
            bool onlyUpcoming = false,
            CancellationToken ct = default
        );
    }
}

