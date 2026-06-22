using System.ComponentModel.DataAnnotations;


namespace HealthCareApp.Shared.Dtos.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
