using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisCore_Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? PatientId { get; set; }
        public Patient? Patient { get; set; }
        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
