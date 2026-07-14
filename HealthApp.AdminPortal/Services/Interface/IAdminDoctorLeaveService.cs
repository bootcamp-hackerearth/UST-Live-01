using HealthApp.AdminPortal.Models;
using HealthApp.Shared.Dtos;

namespace HealthApp.AdminPortal.Services.Interface
{
    public interface IAdminDoctorLeaveService
    {
        Task<ApiResult<List<AdminDoctorLeaveDto>>> GetDoctorLeaves(
            string? search,
            string? status,
            DateOnly? fromDate,
            DateOnly? toDate,
            int pageNumber = 1,
            int pageSize = 100);
    }
}