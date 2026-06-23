using HealthCareApp.AdminBlazor.Services.Interfaces;
using HealthCareApp.Shared.Dtos.Dashboard;
using Microsoft.JSInterop;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class AdminDashboardService : BaseApiService, IAdminDashboardService
    {
        private const string DashboardEndpoint = "api/Admin/dashboard";

        public AdminDashboardService(
            HttpClient httpClient,
            IJSRuntime jsRuntime)
            : base(httpClient, jsRuntime)
        {
        }

        public async Task<AdminDashboardReportDto> GetDashboardReportAsync()
        {
            return await GetAuthorizedAsync<AdminDashboardReportDto>(
                DashboardEndpoint,
                "Your admin session is not authorized to load dashboard data.");
        }
    }
}