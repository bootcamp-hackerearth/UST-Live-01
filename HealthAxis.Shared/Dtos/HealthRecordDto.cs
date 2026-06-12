using System;

namespace HealthAxis.Shared.Dtos
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; }

        public string Prescription { get; set; }

        public string Notes { get; set; }

        public string DoctorName { get; set; }

        public string Specialisation { get; set; }
    }
}