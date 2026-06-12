using System;

namespace HealthCareApi.Models
{
    public class HealthRecord
    {
        public int RecordId { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Appointment Appointment { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}