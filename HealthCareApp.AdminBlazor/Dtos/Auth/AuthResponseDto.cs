namespace HealthCareApp.AdminBlazor.Dtos.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }
    }
}