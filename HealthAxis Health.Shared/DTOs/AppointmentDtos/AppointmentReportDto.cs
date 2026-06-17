using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.Shared.DTOs.ReportDtos
{
    [ExcludeFromCodeCoverage]
    public class AppointmentReportDto
    {
        public DateTime Date { get; set; }

        public int ConfirmedCount { get; set; }

        public int CancelledCount { get; set; }

        public int CompletedCount { get; set; }
    }
}
