using HealthCareApp.Enums;

namespace HealthCareApp.Dtos
{
    public class DoctorPaginationQueryDto : PaginationQueryDto
    {
        public SpecialisationType? Specialisation { get; set; }

        public bool? IsActive { get; set; }

        public string? SearchTerm { get; set; }
    }
}