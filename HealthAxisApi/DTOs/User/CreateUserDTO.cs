using HealthAxisCore_Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.DTOs.User
{
    public class CreateUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; }

        public int? ReferenceId { get; set; }
    }
}