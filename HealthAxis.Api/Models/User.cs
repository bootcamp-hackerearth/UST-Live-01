using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        [RegularExpression("(Admin|Patient|Doctor)")]
        public string Role { get; set; }
        public int? ReferenceId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
