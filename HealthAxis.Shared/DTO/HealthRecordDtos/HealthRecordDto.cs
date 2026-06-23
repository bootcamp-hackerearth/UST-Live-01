namespace HealthAxis.Shared.DTO.HealthRecordDtos
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }

        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public string Specialisation { get; set; } = string.Empty;

        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; } = string.Empty;

        public string Prescription { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}