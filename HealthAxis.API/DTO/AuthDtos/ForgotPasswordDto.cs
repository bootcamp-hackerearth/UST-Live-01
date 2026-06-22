using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.DTO.AuthDtos
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;
    }
}