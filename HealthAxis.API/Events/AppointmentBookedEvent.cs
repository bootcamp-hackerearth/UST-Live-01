namespace HealthAxis.API.Events
{
    public sealed class AppointmentBookedEvent
    {
        public string EventType { get; set; } = "AppointmentBooked";

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public int DoctorId { get; set; }

        public int AppointmentId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}