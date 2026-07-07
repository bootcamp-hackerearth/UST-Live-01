using HealthCareApp.Shared.Dtos.Auth;

namespace HealthCareApp.Models
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }

        public static implicit operator AuthResponse(AuthResponseDto v)
        {
            throw new NotImplementedException();
        }
    }
}