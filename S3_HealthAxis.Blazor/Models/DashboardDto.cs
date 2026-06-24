namespace S3_HealthAxis.Blazor.Models
{
    public class DashboardDto
    {
        public int TotalDoctors { get; set; }
        public int ActiveDoctors { get; set; }

        public int TotalPatients { get; set; }
        public int ActivePatients { get; set; }

        public int TodayAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public int CompletedAppointments { get; set; }
    }
}