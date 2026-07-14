using HealthApp.Shared.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IDoctorLeaveService
    {
        Task<DoctorLeaveDto> CreateMyLeaveAsync(
            DoctorLeaveCreateDto dto,
            string identityUserId);

        Task<List<DoctorLeaveDto>> GetMyLeavesAsync(
            string identityUserId);

        Task<List<DoctorLeaveDto>> GetLeavesByDoctorIdAsync(
            int doctorId);

        Task<bool> IsDoctorOnLeaveAsync(int doctorId,
            DateTime date);

        Task<(List<DoctorLeaveDto> Items, int TotalCount)> GetAllLeaveDoctorAsync( int pageNumber, int pageSize);
    }
}