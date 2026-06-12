using HealthCareApi.Models;
using System;

namespace HealthCareApi.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; }
        public string Status { get; set; }        // "Confirmed", "Pending", "Cancelled", "Completed"
        public string CancellationReason { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual HealthRecord HealthRecord { get; set; }
    }
}