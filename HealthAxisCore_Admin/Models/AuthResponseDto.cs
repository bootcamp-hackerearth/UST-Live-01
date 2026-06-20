namespace HealthAxisCore_Admin.Models
{
    public class AuthResponseDto
    {
        public string UserId { get; set; } = string.Empty;

        public int? PatientId { get; set; }

        public int? DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }
    }
}