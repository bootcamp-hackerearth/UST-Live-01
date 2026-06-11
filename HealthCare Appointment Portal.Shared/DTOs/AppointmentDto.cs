using HealthCare_Appointment_Portal.Enums;
using System;

namespace HealthCare_Appointment_Portal.DTOs.AppointmentDtos
{
    public class AppointmentDto
    {
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

        public DateTime ScheduledDate
        {
            get;
            set;
        }

        public string TimeSlot
        {
            get;
            set;
        }

        public bool HasHealthRecord 

        { 
            get;
            set;
        
        }
        public AppointmentStatus Status
        {
            get;
            set;
        }

        public string CancellationReason
        {
            get;
            set;
        }
    }

}