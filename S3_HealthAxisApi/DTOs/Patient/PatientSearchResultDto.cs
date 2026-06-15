namespace S3_HealthAxisApi.DTOs.Patient
{
    public class PatientSearchResultDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
