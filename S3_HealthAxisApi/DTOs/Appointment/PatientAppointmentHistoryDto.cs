namespace S3_HealthAxisApi.DTOs.Appointment
{
    public class PatientAppointmentHistoryDto
    {
        public int AppointmentId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int TimeSlot { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
