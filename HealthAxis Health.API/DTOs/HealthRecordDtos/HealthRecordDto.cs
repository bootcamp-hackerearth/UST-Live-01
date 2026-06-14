using HealthAxisHealth.API.Enums;

namespace HealthAxisHealth.API.DTOs.HealthRecordDtos
{

    public class HealthRecordDto
    {

        #region Properties

        public int RecordId { get; set; }

        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public Specialisation Specialisation { get; set; }

        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; } = string.Empty;

        public string Prescription { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        #endregion
    }
}
