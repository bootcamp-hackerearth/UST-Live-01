namespace S3_HealthAxisApi.DTOs.Appointment
{
    public class UpdateAppointmentStatusDto
    {
        public int Status { get; set; }

        public string? CancellationReason { get; set; }
    }
}