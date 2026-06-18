namespace S3_HealthAxisApi.DTOs.Admin
{
    public class AdminDashboardDto
    {
        public int TotalPatients { get; set; }

        public int ActivePatients { get; set; }

        public int TotalDoctors { get; set; }

        public int ActiveDoctors { get; set; }

        public int TodayAppointments { get; set; }

        public int PendingAppointments { get; set; }

        public int CompletedAppointments { get; set; }
    }
}