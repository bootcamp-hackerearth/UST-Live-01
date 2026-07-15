using HealthCareApp.Shared.Dtos.DoctorLeaves;

namespace HealthCareApp.AdminBlazor.Services.Interfaces
{
    public interface IDoctorLeaveAdminService
    {
        Task<List<DoctorLeaveDto>> GetDoctorLeavesAsync(int doctorId);
    }
}