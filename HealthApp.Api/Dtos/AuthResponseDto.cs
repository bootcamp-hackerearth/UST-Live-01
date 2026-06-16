namespace HealthApp.Api.Dtos
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string Message { get; set; }
        public int ExpiresIn { get; set; }
    }
}
