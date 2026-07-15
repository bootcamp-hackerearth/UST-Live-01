using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Repositories.Interface
{
    public interface IDoctorLeaveRepository
    {
        Task AddAsync(DoctorLeave leave);

        Task<List<DoctorLeave>> GetByDoctorIdAsync(int doctorId);

        Task<bool> HasOverlappingLeaveAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate);

        Task<DoctorLeave?> GetActiveLeaveForDateAsync(
            int doctorId,
            DateTime date);

        Task<List<DoctorLeave>> GetExpiredLeavesAsync();

        Task SaveChangesAsync();
    }
}