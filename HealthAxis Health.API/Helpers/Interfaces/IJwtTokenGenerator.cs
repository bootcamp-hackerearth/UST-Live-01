using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Helpers
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(
            User user);

        string GenerateRefreshToken();
    }
}
