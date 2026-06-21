namespace HealthCareApp.AdminBlazor.Dtos.Dashboard
{
    public class AdminDashboardReportDto
    {
        public int TotalDoctors { get; set; }

        public int TotalPatients { get; set; }

        public int TotalAppointments { get; set; }


        public int TodaysAppointments { get; set; }

        public int TodaysPatients { get; set; }

        public int CompletedToday { get; set; }

        public int PendingToday { get; set; }

        public int CancelledToday { get; set; }
    }
}