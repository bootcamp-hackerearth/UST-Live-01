namespace S3_HealthAxis.Blazor.Models
{
    public class StatisticsDto
    {
        public int CompletedAppointments { get; set; }

        public int PendingAppointments { get; set; }

        public int ConfirmedAppointments { get; set; }

        public int CancelledAppointments { get; set; }
    }
}