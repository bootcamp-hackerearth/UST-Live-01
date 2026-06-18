using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.DTOs.User
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
