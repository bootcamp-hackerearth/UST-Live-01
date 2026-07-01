using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models.Dtos
{
    public class CreateAdminHandoffResponseDto
    {
        public string Code { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }

    public class ExchangeAdminHandoffRequestDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;
    }
}