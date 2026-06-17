namespace HealthAxis.API.Models.Auth
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string Message { get; set; }
        public int ExpiresIn { get; set; }
    }
}