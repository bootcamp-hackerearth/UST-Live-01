namespace HealthCare.Api.DTOs.Appointments
{
    public class AppointmentReportDto
    {
        public DateOnly Date { get; set; }
        public int PendingCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int CancelledCount { get; set; }
        public int CompletedCount { get; set; }
    }
}
