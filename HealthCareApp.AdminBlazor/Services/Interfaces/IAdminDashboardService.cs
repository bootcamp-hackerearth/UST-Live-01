using HealthCareApp.AdminBlazor.Dtos.Dashboard;

namespace HealthCareApp.AdminBlazor.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardReportDto> GetDashboardReportAsync();
    }
}