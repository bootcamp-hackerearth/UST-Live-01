namespace HealthAxisCore_Api.DTOs.User
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;

        public int? ReferenceId { get; set; }

        public bool IsFirstLogin { get; set; }
    }
}