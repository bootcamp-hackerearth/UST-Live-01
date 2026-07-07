using HealthCareApp.Shared.Enums;


namespace HealthCareApp.Shared.Dtos.Appointments
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string? PatientName { get; set; }

        public int DoctorId { get; set; }

        public string? DoctorName { get; set; }

        public string ScheduledDate { get; set; } = string.Empty;

        public string TimeSlot { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; }

        public string? CancellationReason { get; set; }
    }
}