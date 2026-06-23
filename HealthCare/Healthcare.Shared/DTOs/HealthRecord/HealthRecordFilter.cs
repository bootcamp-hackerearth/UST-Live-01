namespace Healthcare.Shared.DTOs.HealthRecord
{
    public class HealthRecordFilter: PaginationParam
    {
        public DateOnly? VisitDate { get; set; }
    }
}
