

using SharedClasses.Enums;
using System;
namespace SharedClasses.Dtos
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }
        
        public DateTime ScheduledDate { get; set; }

        public int SlotNumber { get; set; }

        public AppointmentStatus Status { get; set; }

        public string CancellationReason { get; set; }
    }
}


