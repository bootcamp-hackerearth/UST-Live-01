using S3_HealthAxis.Blazor.Services;
using S3_HealthAxis.Blazor.Models;
using System.Net.Http.Json;

namespace S3_HealthAxis.Blazor.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;

        public AdminService(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DashboardDto?>
            GetDashboardAsync()
        {
            return await _httpClient
                .GetFromJsonAsync<DashboardDto>(
                    "api/admin/dashboard");
        }

        public async Task<StatisticsDto?>
            GetStatisticsAsync()
        {
            return await _httpClient
                .GetFromJsonAsync<StatisticsDto>(
                    "api/admin/statistics");
        }
    }
}