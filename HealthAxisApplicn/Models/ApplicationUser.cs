using Microsoft.AspNetCore.Identity;

namespace HealthAxisApplicn.Models
{
    public class ApplicationUser: IdentityUser
    {
        public bool IsFirstLogin { get; set; } = true;
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
