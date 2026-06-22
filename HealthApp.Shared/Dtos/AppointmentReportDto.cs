namespace HealthApp.Shared.Dtos
{
    public class AppointmentReportDto
    {
        public DateOnly Date { get; set; }
        public int Confirmed { get; set; }
        public int Cancelled { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int Total { get; set; }
    }
}