namespace S3_HealthAxisApi.DTOs.Doctor
{
    public class DoctorCreationResultDto
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string TemporaryPassword { get; set; } = string.Empty;
    }
}