using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.CommonDtos
{
    public sealed class PaginationQueryDto
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 50)]
        public int PageSize { get; set; } = 6;
    }
}