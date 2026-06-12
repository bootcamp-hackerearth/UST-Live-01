using HealthAxis.Shared.Dtos;
using System;

namespace HealthAxis.Shared.Dtos
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; }
        public AppointmentStatus Status { get; set; }

        public string CancellationReason { get; set; }
        public string CancelledBy { get; set; }

        public string PatientName { get; set; }
        public string DoctorName { get; set; }
    }

    public class BookAppointmentDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialisation { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; }
    }

    public class UpdateAppointmentStatusDto
    {
        public AppointmentStatus Status { get; set; }
        public string CancellationReason { get; set; }
        public string CancelledBy { get; set; }
    }
}