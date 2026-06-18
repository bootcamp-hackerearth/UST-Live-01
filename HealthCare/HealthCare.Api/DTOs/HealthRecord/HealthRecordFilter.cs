namespace HealthCare.Api.DTOs.HealthRecord
{
    public class HealthRecordFilter: PaginationParam
    {
        public DateOnly? VisitDate { get; set; }
    }
}
