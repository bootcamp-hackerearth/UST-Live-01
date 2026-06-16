using HealthAxisCore_Api.Enums;

namespace HealthAxisCore_Api.DTOs.Appointment
{
    public class AppointmentResponseDTO
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = null!;

        public AppointmentStatus Status { get; set; }

        public string? CancellationReason { get; set; }
    }
}
