namespace S3_HealthAxis.Shared.DTOs.Auth
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public int Role { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public bool IsActive { get; set; }
    }
}
