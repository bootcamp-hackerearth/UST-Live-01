using Microsoft.AspNetCore.Identity;

namespace HealthApp.API.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool MustChangePassword { get; set; } = false;
}
