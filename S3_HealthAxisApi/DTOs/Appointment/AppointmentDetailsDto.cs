namespace S3_HealthAxisApi.DTOs.Appointment
{
    public class AppointmentDetailsDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateOnly ScheduledDate { get; set; }
        public int TimeSlot { get; set; }
        public int Status { get; set; }
        public string? CancellationReason { get; set; }
    }
}
