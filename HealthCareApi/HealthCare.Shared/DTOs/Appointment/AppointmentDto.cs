using System;

namespace HealthCare.Shared.DTOs.Appointment
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; }
        public string Status { get; set; }
        public string CancellationReason { get; set; }
    }
}