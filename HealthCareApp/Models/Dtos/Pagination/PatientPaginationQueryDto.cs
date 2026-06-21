using HealthCareApp.Enums;

namespace HealthCareApp.Dtos
{
    public class PatientPaginationQueryDto : PaginationQueryDto
    {
        public string? SearchTerm { get; set; }

        public GenderType? Gender { get; set; }

        public bool? HasInsurance { get; set; }
    }
}