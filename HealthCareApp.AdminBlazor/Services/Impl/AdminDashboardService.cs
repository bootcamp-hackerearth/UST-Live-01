using HealthCareApp.Shared.Dtos.Dashboard;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using HealthCareApp.AdminBlazor.Services.Interfaces;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class AdminDashboardService : BaseApiService, IAdminDashboardService
    {
        private const string DashboardEndpoint = "api/Admin/dashboard";

        public AdminDashboardService(
      HttpClient httpClient,
      IJSRuntime jsRuntime,
      NavigationManager navigationManager)
      : base(httpClient, jsRuntime, navigationManager)
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