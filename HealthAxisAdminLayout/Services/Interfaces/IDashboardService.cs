using HealthAxisAdminLayout.DTOs.Dashboard;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<AdminDashboardDto> GetDashboardAsync();
    }
}

