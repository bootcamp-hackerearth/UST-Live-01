using Microsoft.AspNetCore.Identity;

namespace HealthAxisApplicn.Models
{
    public class ApplicationUser: IdentityUser
    {
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
