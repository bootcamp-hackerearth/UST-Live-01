namespace HealthAxis.Shared.DTOs.Doctor;

public sealed class DoctorAvailabilityDto
{
    public int DoctorId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime Date { get; set; }

    public List<string> AvailableSlots { get; set; } = new();
}