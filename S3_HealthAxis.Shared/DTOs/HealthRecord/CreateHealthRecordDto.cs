namespace S3_HealthAxis.Shared.DTOs.HealthRecord
{
    public class CreateHealthRecordDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string? Diagnosis { get; set; }
        public string? Prescription { get; set; }
        public string? Notes { get; set; }
    }
}
