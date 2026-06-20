using HealthAxisCore_Admin.Models;

namespace HealthAxisCore_Admin.Services
{
    public class ReportService
    {
        private static readonly List<AppointmentReportDto> Reports = new()
        {
            new AppointmentReportDto
            {
                Date = DateTime.Today.AddDays(-4),
                Confirmed = 8,
                Cancelled = 1,
                Completed = 5
            },
            new AppointmentReportDto
            {
                Date = DateTime.Today.AddDays(-3),
                Confirmed = 10,
                Cancelled = 2,
                Completed = 7
            },
            new AppointmentReportDto
            {
                Date = DateTime.Today.AddDays(-2),
                Confirmed = 12,
                Cancelled = 1,
                Completed = 9
            },
            new AppointmentReportDto
            {
                Date = DateTime.Today.AddDays(-1),
                Confirmed = 9,
                Cancelled = 3,
                Completed = 6
            },
            new AppointmentReportDto
            {
                Date = DateTime.Today,
                Confirmed = 14,
                Cancelled = 0,
                Completed = 4
            }
        };

        public Task<List<AppointmentReportDto>> GetAppointmentReportsAsync()
        {
            /*
             * ============================================================
             * TEMPORARY DISCONNECTED VERSION
             * ============================================================
             * Returns hardcoded reports instead of calling:
             * GET api/admin/reports/appointments
             * ============================================================
             */

            return Task.FromResult(
                Reports
                    .OrderByDescending(report => report.Date)
                    .ToList());
        }
    }
}