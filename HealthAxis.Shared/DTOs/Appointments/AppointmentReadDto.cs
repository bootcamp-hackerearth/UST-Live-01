using HealthAxis.API.Enums;

namespace HealthAxis.API.DTOs.Appointments
{
    public class AppointmentReadDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; }

        public string CancellationReason { get; set; } = string.Empty;
    }
}
