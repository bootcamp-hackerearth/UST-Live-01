using HealthAxis.Shared.DTOs.DoctorLeaves;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IDoctorLeaveService
    {
        Task<DoctorLeaveDto> CreateLeaveAsync(
            int doctorId,
            CreateMyDoctorLeaveDto dto);

        Task<List<DoctorLeaveDto>> GetDoctorLeavesAsync(
            int doctorId);
    }
}