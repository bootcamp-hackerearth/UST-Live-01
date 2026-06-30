namespace HealthCareApp.Shared.Dtos.Pagination
{
    public class HealthRecordPaginationQueryDto : PaginationQueryDto
    {
        public string? SearchTerm { get; set; }

        public DateTime? VisitDate { get; set; }
    }
}