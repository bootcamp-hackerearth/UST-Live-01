namespace HealthApp.API.Models;

public class Notification
{
    public int NotificationId { get; set; }

    public int DoctorId { get; set; }

    public int AppointmentId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public Doctor? Doctor { get; set; }

    public Appointment? Appointment { get; set; }
}
