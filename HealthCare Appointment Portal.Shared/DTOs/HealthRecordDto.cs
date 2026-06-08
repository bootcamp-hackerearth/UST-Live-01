using System;

namespace HealthCare_Appointment_Portal.DTOs.HealthRecordDtos
{
    public class HealthRecordDto
    {
        public int RecordId
        {
            get;
            set;
        }

        public int AppointmentId
        {
            get;
            set;
        }

        public int PatientId
        {
            get;
            set;
        }

        public string PatientName
        {
            get;
            set;
        }

        public int DoctorId
        {
            get;
            set;
        }

        public string DoctorName
        {
            get;
            set;
        }

        public string Specialisation
        {
            get;
            set;
        }

        public DateTime VisitDate
        {
            get;
            set;
        }

        public string Diagnosis
        {
            get;
            set;
        }

        public string Prescription
        {
            get;
            set;
        }

        public string Notes
        {
            get;
            set;
        }
    }
}