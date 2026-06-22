namespace S3_HealthAxis.Shared.DTOs.Appointment
{
    public class PatientAppointmentHistoryDto
    {
        public int AppointmentId { get; set; }
        public DateOnly ScheduledDate { get; set; }
        public int TimeSlot { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
