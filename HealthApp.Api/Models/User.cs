using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [RegularExpression("^(Patient|Doctor|Admin)$")]
        public string Role { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
