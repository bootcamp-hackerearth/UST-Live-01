namespace HealthCareApp.Shared.Dtos.PatientNotifications
{
    public class PatientNotificationDto
    {
        public int PatientNotificationId { get; set; }

        public int PatientId { get; set; }

        public int? DoctorId { get; set; }

        public int? AppointmentId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string NotificationType { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}