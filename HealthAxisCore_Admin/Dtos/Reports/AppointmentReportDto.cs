namespace HealthAxisCore_Admin.Dtos.Reports;

public class AppointmentReportDto
{
    public DateTime Date { get; set; }

    public int Confirmed { get; set; }

    public int Cancelled { get; set; }

    public int Completed { get; set; }

    public int Total => Confirmed + Cancelled + Completed;
}