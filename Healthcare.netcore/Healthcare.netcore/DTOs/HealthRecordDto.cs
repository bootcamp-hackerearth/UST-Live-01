namespace HealthAxis.API.Dtos.HealthRecordDtos;

public class HealthRecordDto
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public string Diagnosis { get; set; } = string.Empty;

    public string Prescription { get; set; } = string.Empty;

    public string? Notes { get; set; }
}