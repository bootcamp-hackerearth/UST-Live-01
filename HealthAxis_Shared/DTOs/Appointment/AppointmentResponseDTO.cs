using HealthAxis.Shared.Enums;
using System;

namespace HealthAxis.Shared.DTOs.Appointment
{
    public class AppointmentResponseDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; }

        public string? CancellationReason { get; set; }
    }
}