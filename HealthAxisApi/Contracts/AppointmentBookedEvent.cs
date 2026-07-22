namespace HealthAxisCore_Api.Contracts
{
    public sealed class AppointmentBookedEvent
    {
        public Guid EventId { get; init; }

        public int AppointmentId { get; init; }

        public int PatientId { get; init; }

        public string PatientName { get; init; } =
            string.Empty;

        public int DoctorId { get; init; }

        public DateTime ScheduledDate { get; init; }

        public string TimeSlot { get; init; } =
            string.Empty;
    }
}