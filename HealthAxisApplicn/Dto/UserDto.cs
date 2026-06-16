using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        [Required]
        public required string PasswordSalt { get; set; }
        [RegularExpression("(Admin|Doctor|Patient)")]
        public string Role { get; set; }
        public int? ReferenceId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
