namespace S3_HealthAxisApi.DTOs.Appointment
{
    public class UpdateAppointmentDto
    {
        public int DoctorId { get; set; }
        public DateOnly ScheduledDate { get; set; }
        public int TimeSlot { get; set; }
    }

}
