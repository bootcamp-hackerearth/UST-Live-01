namespace HealthApp.Shared.Dtos
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
