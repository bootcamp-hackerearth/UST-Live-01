namespace Healthcare.Shared.DTOs.Appointments
{
    public class AppointmentListDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int? HealthRecordId { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; } 
        public DateOnly ScheduledDate { get; set; }
        public string TimeSlot { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
