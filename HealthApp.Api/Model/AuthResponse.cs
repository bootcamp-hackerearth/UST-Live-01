namespace HealthApp.Api.Model
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }=string.Empty;
        public string message { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }

    }
}
