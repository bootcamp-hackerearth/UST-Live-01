namespace HealthApp.Shared.Dto
{
    public class LoginResponseDto
    {

        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public string Role { get; set; } = string.Empty;

    }
}