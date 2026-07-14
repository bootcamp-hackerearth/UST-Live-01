using HealthApp.API.Models;

namespace HealthApp.API.Repository.Interface;

public interface IDoctorLeaveRepository : IRepository<DoctorLeave>
{
    Task<List<DoctorLeave>> GetByDoctorIdAsync(
        int doctorId,
        CancellationToken ct = default);

    Task<bool> HasOverlappingLeaveAsync(
        int doctorId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default);

    Task<DoctorLeave?> GetLeaveForDateAsync(
        int doctorId,
        DateTime date,
        CancellationToken ct = default);
}