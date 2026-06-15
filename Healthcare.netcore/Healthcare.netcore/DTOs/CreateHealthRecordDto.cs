using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Dtos.HealthRecordDtos;

public class CreateHealthRecordDto
{
    [Required]
    public int PatientId { get; set; }

    [Required]
    public int DoctorId { get; set; }

    [Required]
    public string Diagnosis { get; set; } = string.Empty;

    [Required]
    public string Prescription { get; set; } = string.Empty;

    public string? Notes { get; set; }
}