namespace S3_HealthAxisApi.DTOs.Patient
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? InsuranceId { get; set; }
        public bool IsActive { get; set; }
    }
}
