using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dto

{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }=string.Empty;
        [Required]
        public string Password { get; set; }= string.Empty;
    }
}
