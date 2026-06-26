namespace HealthAxis.Shared.DTOs.User
{
    public class AuthResponseDTO
    {
        // ✅ Access Token
        public string Token { get; set; } = string.Empty;

        // ✅ Refresh Token
        public string? RefreshToken { get; set; }

        // ✅ User Info
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int? ReferenceId { get; set; }

        public bool IsFirstLogin { get; set; }
    }
}
