namespace S3_HealthAxisApi.DTOs.Patient
{
    public class PatientSummaryDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; } = string.Empty;
    }
}