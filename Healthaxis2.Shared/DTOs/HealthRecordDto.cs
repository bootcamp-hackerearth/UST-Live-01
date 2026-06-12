using System;

namespace Healthaxis2.Shared.DTOs
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }

        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; }

        public string Prescription { get; set; }

        public string Notes { get; set; }

        // ✅ EXTRA (for UI / mapping)
        public string DoctorName { get; set; }

        public string PatientName { get; set; }
    }
}