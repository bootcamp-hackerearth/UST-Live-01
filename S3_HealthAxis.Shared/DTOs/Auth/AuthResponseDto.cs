namespace S3_HealthAxis.Shared.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
        public bool MustChangePassword { get; set; }

        public int? ReferenceId { get; set; }
    }
}