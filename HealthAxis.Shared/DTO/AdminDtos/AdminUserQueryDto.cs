using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.AdminDtos
{
    public sealed class AdminUserQueryDto
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 50)]
        public int PageSize { get; set; } = 6;

        public string SearchText { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}