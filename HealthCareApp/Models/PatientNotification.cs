namespace HealthCareApp.Models
{
    public class PatientNotification
    {
        public int PatientNotificationId { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        public int? DoctorId { get; set; }

        public Doctor? Doctor { get; set; }

        public int? AppointmentId { get; set; }

        public Appointment? Appointment { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string NotificationType { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}