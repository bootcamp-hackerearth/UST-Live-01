using HealthCareApp.Shared.Enums;

namespace HealthCareApp.Shared.Dtos.Pagination
{
    public class PatientPaginationQueryDto : PaginationQueryDto
    {
        public string? SearchTerm { get; set; }

        public GenderType? Gender { get; set; }

        public bool? HasInsurance { get; set; }
    }
}