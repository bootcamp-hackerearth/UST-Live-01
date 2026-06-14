using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [RegularExpression("(Patient|Doctor|Admin)")]
        public string Role { get; set; }
        
        [Required]
        public int ReferenceId { get; set; }
    }
}
