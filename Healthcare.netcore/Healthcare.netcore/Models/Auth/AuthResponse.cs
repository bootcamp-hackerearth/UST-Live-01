namespace HealthAxis.API.Models.Auth
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }

        public bool RequiresPasswordChange { get; set; } = false;
    }
}