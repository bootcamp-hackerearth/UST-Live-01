namespace S3_HealthAxisApi.DTOs.Patient
{
    public class UpdatePatientDto
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? InsuranceId { get; set; }
    }
}
