using HealthAxisApplicn.Dto.Auth;

namespace HealthAxisAdminPortal.Services.Interfaces
{

    public interface IAuthApiService
    {
        Task<AuthResponse?> LoginAsync(LoginDto dto);
    }

}
