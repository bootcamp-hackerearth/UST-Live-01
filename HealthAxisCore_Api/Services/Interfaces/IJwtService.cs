using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);

        string GenerateRefreshToken();
    }
}