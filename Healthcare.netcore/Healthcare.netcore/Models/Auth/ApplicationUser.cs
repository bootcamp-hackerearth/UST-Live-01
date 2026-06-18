using Microsoft.AspNetCore.Identity;

namespace HealthAxis.API.Models.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public bool MustChangePassword { get; set; } = false;
    }
}