using HealthCareApp.Models;

namespace HealthCareApp.Repository.Interface
{
    public interface IDoctorLeaveRepository : IRepository<DoctorLeave>
    {
        Task<List<DoctorLeave>> GetLeavesByDoctorIdAsync(int doctorId);

        Task<bool> IsDoctorOnLeaveAsync(int doctorId, DateTime date);

        Task<bool> HasOverlappingLeaveAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate);
    }
}