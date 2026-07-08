namespace HealthAxis.API.Models;

public sealed class Notification
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}