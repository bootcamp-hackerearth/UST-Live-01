using HealthCareApp.AdminBlazor.Services.Interfaces;
using HealthCareApp.Shared.Dtos.Dashboard;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class AdminDashboardService : IAdminDashboardService
    {
        public Task<AdminDashboardReportDto> GetDashboardReportAsync()
        {
            var report = new AdminDashboardReportDto
            {
                TotalDoctors = 12,
                TotalPatients = 48,
                TotalAppointments = 26,

                TodaysAppointments = 8,
                TodaysPatients = 5,
                CompletedToday = 3,
                PendingToday = 4,
                CancelledToday = 1
            };

            return Task.FromResult(report);
        }
    }
}