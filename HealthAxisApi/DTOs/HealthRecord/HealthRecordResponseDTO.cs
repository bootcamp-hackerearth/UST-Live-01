namespace HealthAxisCore_Api.DTOs.HealthRecord
{
    public class HealthRecordResponseDTO
    {
        public int HealthRecordId { get; set; }

        public int PatientId { get; set; }

        public int? DoctorId { get; set; }

        public int AppointmentId { get; set; }

        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; } = null!;

        public string Prescription { get; set; } = null!;

        public string? Notes { get; set; }
    }
}