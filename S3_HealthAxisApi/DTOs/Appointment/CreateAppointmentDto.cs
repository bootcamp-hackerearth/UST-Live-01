namespace S3_HealthAxisApi.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int TimeSlot { get; set; }
    }

}
