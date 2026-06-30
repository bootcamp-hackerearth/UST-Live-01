using HealthAxis.API.Enums;

namespace HealthAxis.API.DTOs.HealthRecords
{
    public class HealthRecordReadDto
    {
        public int HealthRecordId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public Specialisation Specialisation { get; set; }

        public int AppointmentId { get; set; }

        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; } = string.Empty;

        public string Prescription { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}
