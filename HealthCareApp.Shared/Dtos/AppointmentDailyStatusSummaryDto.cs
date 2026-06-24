namespace HealthCareApp.Shared.Dtos.Appointments
{
    public class AppointmentDailyStatusSummaryDto
    {
        public string Date { get; set; } = string.Empty;

        public int Total { get; set; }

        public int Pending { get; set; }

        public int Confirmed { get; set; }

        public int Completed { get; set; }

        public int Cancelled { get; set; }
    }
}