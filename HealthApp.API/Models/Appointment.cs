using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models;

public class Appointment
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    [Required] 
    public DateTime ScheduledDate { get; set; }

    [Required] 
    public string TimeSlots { get; set; } = string.Empty;
    [Required, RegularExpression("(Pending|Confirmed|Cancelled|Completed)")]
    public string Status { get; set; } = string.Empty;
    [MaxLength(200)] 
    public string? CancellationReason { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public HealthRecord? HealthRecord { get; set; }
}
