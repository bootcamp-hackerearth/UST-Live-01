using HealthApp.Shared.Dto;

namespace HealthApp.Blazor.Components.service.Interface
{
    public interface IDoctorLeaveService
    {
        Task<PagedResponse<DoctorLeaveDto>> GetAllDoctorLeavesAsync(
            int pageNumber,
            int pageSize);

        Task<List<DoctorLeaveDto>> GetDoctorLeavesAsync(int doctorId);

    }
}