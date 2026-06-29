namespace Healthcare.Shared.DTOs.HealthRecord
{
    public class HealthRecordListDto
    {
        public int RecordId { get; set; }
        public string DoctorName { get; set; } = null!;
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; } = null!;
        public string Prescription { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
