namespace S3_HealthAxis.Shared.DTOs.Patient
{
    public class PatientSummaryDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; } = string.Empty;
    }
}