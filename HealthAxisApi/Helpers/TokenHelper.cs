using System.Security.Cryptography;

namespace HealthAxisCore_Api.Helpers
{
    public static class TokenHelper
    {
        // ✅ Generate secure refresh token
        public static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64]; // 512-bit token

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
