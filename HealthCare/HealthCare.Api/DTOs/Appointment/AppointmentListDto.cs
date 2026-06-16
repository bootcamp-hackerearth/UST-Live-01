namespace HealthCare.Api.DTOs.Appointments
{
    public class AppointmentListDto
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = null!;
        public string DoctorName { get; set; } = null!;
        public DateOnly ScheduledDate { get; set; }
        public string TimeSlot { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
