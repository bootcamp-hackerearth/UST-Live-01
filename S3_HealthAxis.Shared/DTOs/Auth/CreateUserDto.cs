namespace S3_HealthAxis.Shared.DTOs.Auth
{
    public class CreateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Role { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
    }
}
