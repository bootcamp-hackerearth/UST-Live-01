namespace S3_HealthAxis.Blazor.Models
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