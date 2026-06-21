namespace HealthCareApp.AdminBlazor.Dtos.Auth
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}