using HealthAxis.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; }

        [Required]
        public int ReferenceId { get; set; }
    }
}