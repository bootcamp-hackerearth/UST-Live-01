namespace HealthAxisCore_Api.Contracts
{
    public class AppointmentBookedEvent
    {
        public int AppointmentId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int DoctorId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;
    }
}