using S3_HealthAxis.Blazor.Models;
using S3_HealthAxis.Shared.DTOs.Admin;

namespace S3_HealthAxis.Blazor.Services
{
    public interface IAdminService
    {
        Task<DashboardDto?> GetDashboardAsync();

        Task<StatisticsDto?> GetStatisticsAsync();

        Task<List<UserManagementDto>?> GetUsersAsync();
    }
}
