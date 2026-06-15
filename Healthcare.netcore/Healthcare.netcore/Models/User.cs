using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // Admin, Doctor, Patient

        public bool IsActive { get; set; } = true;
    }
}