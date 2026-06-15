namespace S3_HealthAxisApi.DTOs.HealthRecord
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string? Diagnosis { get; set; }
        public string? Prescription { get; set; }
        public string? Notes { get; set; }
    }

}
