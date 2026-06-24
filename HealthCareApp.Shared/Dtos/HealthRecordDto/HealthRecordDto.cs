
namespace HealthCareApp.Shared.Dtos.HealthRecords
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }

        public int PatientId { get; set; }

        public string? PatientName { get; set; }

        public int? DoctorId { get; set; }

        public string? DoctorName { get; set; }

        public int AppointmentId { get; set; }

        public string VisitDate { get; set; } = string.Empty;

        public string Diagnosis { get; set; } = string.Empty;

        public string Prescription { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}