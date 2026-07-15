namespace HealthAxis.Shared.DTO.HealthRecordDtos
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }

        public int RecordId { get; set; }

        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string Specialisation { get; set; } = string.Empty;

        public DateTime VisitDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Diagnosis { get; set; } = string.Empty;

        public string Prescription { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public DateTime? UpdatedDate { get; set; }
    }
}