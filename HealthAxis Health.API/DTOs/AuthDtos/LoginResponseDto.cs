namespace HealthAxisHealth.API.DTOs.AuthDtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string TokenType { get; set; } = "Bearer";

        public int ExpiresIn { get; set; } = 3600;

        public string RefreshToken { get; set; } = string.Empty;

        public LoginUserDto User { get; set; } = new();
    }

    public class LoginUserDto
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();
    }
}