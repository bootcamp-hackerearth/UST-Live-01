using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Shared.DTOs.Appointment
{
    public class ViewAppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; }

        public int DoctorId { get; set; }
        public string DoctorName { get; set; }

        public string Specialisation { get; set; }

        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; }

        public string Status { get; set; }
        public string CancellationReason { get; set; }
    }
}