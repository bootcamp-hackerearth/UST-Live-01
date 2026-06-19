using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Dto.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required] public string UserId { get; set; } = string.Empty;
        [Required] public string RefreshToken { get; set; } = string.Empty;
    }
}
