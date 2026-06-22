namespace S3_HealthAxis.Shared.DTOs.HealthRecord
{
    public class UpdateHealthRecordDto
    {
        public string? Diagnosis { get; set; }
        public string? Prescription { get; set; }
        public string? Notes { get; set; }
    }
}
