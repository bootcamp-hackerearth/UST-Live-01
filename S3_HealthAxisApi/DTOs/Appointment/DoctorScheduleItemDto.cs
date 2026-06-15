namespace S3_HealthAxisApi.DTOs.Appointment
{
    public class DoctorScheduleItemDto
    {
        public int AppointmentId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int TimeSlot { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
