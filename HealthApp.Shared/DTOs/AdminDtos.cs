namespace HealthApp.Shared.DTOs;

public class AppointmentReportDto
{
    public DateTime Date { get; set; }
    public int Confirmed { get; set; }
    public int Cancelled { get; set; }
    public int Completed { get; set; }
    public int Pending { get; set; }
}
public class UserDto 
{ 
    public string UserId { get; set; } = string.Empty; 
    public string Email { get; set; } = string.Empty; 
    public string FullName { get; set; } = string.Empty; 
    public string Role { get; set; } = string.Empty; 
}
