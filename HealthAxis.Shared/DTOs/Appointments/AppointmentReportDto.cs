namespace HealthAxis.API.DTOs.Appointments
{
    public class AppointmentReportDto
    {
        public DateTime Date { get; set; }

        public int TotalCount { get; set; }

        public int ScheduledCount { get; set; }

        public int ConfirmedCount { get; set; }

        public int CancelledCount { get; set; }

        public int CompletedCount { get; set; }
    }
}
