namespace HealthAxisApplicn.Messaging.Contracts
{
    public class BookAppointmentEvent
    {
        public Guid EventId { get; set; }

        public string EventType { get; set; } = string.Empty;

        public DateTime OccurredAt { get; set; }

        public string Source { get; set; } = string.Empty;

        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;
    }

}
