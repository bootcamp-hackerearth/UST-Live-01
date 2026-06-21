namespace HealthCareApp.AdminBlazor.Dtos.Appointments
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public AppointmentStatusDto Status { get; set; }

        public string? CancellationReason { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}