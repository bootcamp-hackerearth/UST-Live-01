namespace S3_HealthAxis.Shared.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly ScheduledDate { get; set; }
        public int TimeSlot { get; set; }
    }

}
