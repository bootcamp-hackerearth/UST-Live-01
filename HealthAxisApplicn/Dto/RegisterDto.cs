using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(8, ErrorMessage = "Password should be atleast 8 digits")]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;


    }
}
