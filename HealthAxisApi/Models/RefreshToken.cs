using System;

namespace HealthAxisCore_Api.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        // ✅ Actual token value
        public string Token { get; set; } = string.Empty;

        // ✅ Expiration time (e.g., 7 days)
        public DateTime Expires { get; set; }

        // ✅ Check if token expired
        public bool IsExpired => DateTime.UtcNow >= Expires;

        // ✅ Token creation time
        public DateTime Created { get; set; } = DateTime.UtcNow;

        // ✅ Optional: store IP
        public string? CreatedByIp { get; set; }

        // ✅ Revoke support
        public bool IsRevoked { get; set; } = false;

        public DateTime? Revoked { get; set; }

        // ✅ Identity user mapping (IMPORTANT)
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}