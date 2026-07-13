using HealthApp.Api.Models;
using HealthApp.Shared.Dtos;

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
            CancellationToken ct = default);

        Task<(IEnumerable<Appointment> Items, int TotalCount)> GetAppointmentsAsync(
            AppointmentFilterDto filter,
            CancellationToken ct = default);

        Task<List<Appointment>> GetActiveAppointmentsForDoctorDateRangeAsync(
            int doctorId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken ct = default);

        Task CancelAppointmentsAsync(
            IEnumerable<Appointment> appointments,
            string cancellationReason,
            CancellationToken ct = default);
    }
}
