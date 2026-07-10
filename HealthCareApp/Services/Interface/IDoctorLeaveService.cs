using HealthCareApp.Shared.Dtos.DoctorLeaves;
using HealthCareApp.Shared.Dtos.Notifications;

namespace HealthCareApp.Services.Interface
{
    public interface IDoctorLeaveService
    {
        Task<DoctorLeaveDto> CreateMyDoctorLeaveAsync(
            CreateMyDoctorLeaveDto dto,
            string identityUserId);

        Task<List<DoctorLeaveDto>> GetMyDoctorLeavesAsync(
            string identityUserId);

        Task<List<DoctorLeaveDto>> GetDoctorLeavesByDoctorIdAsync(
            int doctorId);

        Task<bool> IsDoctorOnLeaveAsync(
            int doctorId,
            DateTime date);
    }
}