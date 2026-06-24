namespace HealthAxisCore_Admin.Dtos.Dashboard;

public class DashboardSummaryDto
{
    public int TotalDoctors { get; set; }

    public int ActiveDoctors { get; set; }

    public int TotalUsers { get; set; }

    public int ActiveUsers { get; set; }

    public int TotalAppointments { get; set; }

    public int PendingAppointments { get; set; }

    public int CompletedAppointments { get; set; }

    public int CancelledAppointments { get; set; }
}