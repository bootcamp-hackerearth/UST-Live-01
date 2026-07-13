using HealthApp.Api.Models;

namespace HealthApp.Api.Repositories.Interfaces
{
    public interface IDoctorLeaveRepository : IRepository<DoctorLeave>
    {
        Task<IEnumerable<DoctorLeave>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default);

        Task<bool> HasOverlappingLeaveAsync(
            int doctorId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken ct = default);

        Task<DoctorLeave?> GetLeaveForDateAsync(
            int doctorId,
            DateOnly date,
            CancellationToken ct = default);

        Task<bool> IsDoctorOnLeaveAsync(
            int doctorId,
            DateOnly date,
            CancellationToken ct = default);
    }
}
