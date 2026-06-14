using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        public required string PasswordSalt { get; set; }
        [RegularExpression("(Admin|Patient|Doctor)")]
        public required string Role { get; set; }
        public int? ReferenceId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
