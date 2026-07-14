using HealthCareApp.Shared.Dtos.DoctorLeaves;

namespace HealthCareApp.Services
{
    public interface IDoctorLeaveService
    {
        Task<DoctorLeaveDto> CreateMyLeaveAsync(
            string identityUserId,
            CreateDoctorLeaveRequest request);

        Task<List<DoctorLeaveDto>> GetMyLeavesAsync(
            string identityUserId);

        Task<List<DoctorLeaveDto>> GetDoctorLeavesAsync(
            int doctorId);

        Task<DoctorLeaveStatusDto> GetDoctorLeaveStatusAsync(
            int doctorId,
            DateOnly date);

        Task<bool> IsDoctorOnLeaveAsync(
            int doctorId,
            DateOnly date);
    }
}