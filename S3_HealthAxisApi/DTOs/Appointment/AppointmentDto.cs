namespace S3_HealthAxisApi.DTOs.Appointment
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly ScheduledDate { get; set; }
        public int TimeSlot { get; set; }
        public int Status { get; set; }
        public string? CancellationReason { get; set; }
    }
}
