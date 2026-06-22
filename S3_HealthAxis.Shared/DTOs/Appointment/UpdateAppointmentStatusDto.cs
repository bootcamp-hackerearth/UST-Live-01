namespace S3_HealthAxis.Shared.DTOs.Appointment
{
    public class UpdateAppointmentStatusDto
    {
        public int Status { get; set; }

        public string? CancellationReason { get; set; }
    }
}