using HealthAxisApplicn.Dto.Auth;

namespace HealthAxisApplicn.Services
{
    public interface IAuthService
    {
        Task<AuthRegisterResponse> RegisterAsync(RegisterDto request);
        Task<AuthResponse> LoginAsync(LoginDto request);
        Task<AuthResponse?> RefreshAsync(string refreshToken);
    }
}
