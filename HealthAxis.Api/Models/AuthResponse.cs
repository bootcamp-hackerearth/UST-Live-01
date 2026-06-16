namespace HealthAxisCore_Api.Models
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
