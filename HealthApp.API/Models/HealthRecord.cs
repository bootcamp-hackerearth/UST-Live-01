using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models;

public class HealthRecord
{
    public int HealthRecordId { get; set; }
    public int? PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public int? DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    public int? AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;
    [Required] 
    public DateTime VisitDate { get; set; }
    [Required, MaxLength(500)] 
    public string Diagnosis { get; set; } = string.Empty;
    [Required, MaxLength(500)] 
    public string Prescription { get; set; } = string.Empty;
    [MaxLength(1000)] 
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
