using HealthAxis.Shared.Enums;
using HealthAxis.Shared.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace HealthAxis.API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(ValidationLimits.EmailLength)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(25)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; }

        [Required]
        public int ReferenceId { get; set; }
    }
}