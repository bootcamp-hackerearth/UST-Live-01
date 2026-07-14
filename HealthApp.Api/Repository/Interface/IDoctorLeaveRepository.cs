using HealthApp.Api.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface IDoctorLeaveRepository
    {
        Task<DoctorLeave> AddAsync(DoctorLeave leave);

        Task<List<DoctorLeave>> GetByDoctorIdAsync(int doctorId);

        Task<bool> HasOverlappingLeaveAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate);

        Task<bool> IsDoctorOnLeaveAsync(int doctorId, DateTime date);

        Task<(List<DoctorLeave> Items, int TotalCount)> GetAllLeaveDoctorAsync(int pageNumber,int pageSize);
    }
}