using HealthApp.API.Identity;

namespace HealthApp.API.Models;

public class RefreshToken
{
    public int RefreshTokenId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string JwtId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsUsed { get; set; }
}
