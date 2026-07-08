namespace Healthcare.Shared.Events
{
    public class AppointmentBookedEvent
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateOnly ScheduledDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
    }
}
