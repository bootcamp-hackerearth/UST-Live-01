using Microsoft.AspNetCore.Identity;

namespace HealthAxisCore_Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; } = null!;
        public int? ReferenceId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
