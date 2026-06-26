using Microsoft.AspNetCore.Identity;

namespace HealthAxisCore_Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        // ✅ Role (you are using custom role instead of Identity roles)
        public string Role { get; set; } = null!;

        // ✅ Link to Patient / Doctor / Admin
        public int? ReferenceId { get; set; }

        // ✅ Created date
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // ✅ First login check
        public bool IsFirstLogin { get; set; } = false;

        // ✅ Temporary password (for admin-created users)
        public string? TemporaryPassword { get; set; }

        // ✅ NEW → Refresh Tokens (VERY IMPORTANT)
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
