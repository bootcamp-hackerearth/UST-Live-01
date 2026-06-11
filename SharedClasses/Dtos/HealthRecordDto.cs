using System;

namespace SharedClasses.Dtos
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }
       
        public int AppointmentId { get; set; }

        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; }

        public string Prescription { get; set; }

        public string Notes { get; set; }
    }
}