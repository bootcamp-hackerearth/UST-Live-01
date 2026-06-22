using S3_HealthAxis.Blazor.Models;

namespace S3_HealthAxis.Blazor.Services
{
    public interface IAdminService
    {
        Task<DashboardDto?> GetDashboardAsync();

        Task<StatisticsDto?> GetStatisticsAsync();
    }
}