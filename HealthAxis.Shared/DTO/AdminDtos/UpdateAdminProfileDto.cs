using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.AdminDtos
{
    public sealed class UpdateAdminProfileDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}