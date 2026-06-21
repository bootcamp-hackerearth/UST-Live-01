using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models.DTOs;

public class HealthRecordDto
{
    public int HealthRecordId { get; set; }
    public int PatientId { get; set; }
    public string? PatientName { get; set; }
    public int DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public int AppointmentId { get; set; }
    public DateTime VisitDate { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Prescription { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
public class AddHealthRecordDto
{
    [Range(1,int.MaxValue)] public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    [Range(1,int.MaxValue)] public int AppointmentId { get; set; }
    [Required, StringLength(500)] public string Diagnosis { get; set; } = string.Empty;
    [Required, StringLength(500)] public string Prescription { get; set; } = string.Empty;
    [StringLength(1000)] public string? Notes { get; set; }
    [Required] public DateTime VisitDate { get; set; }
}
