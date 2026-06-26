using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.DTOs.User
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
