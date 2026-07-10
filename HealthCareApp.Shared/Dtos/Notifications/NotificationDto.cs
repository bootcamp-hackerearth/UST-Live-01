namespace HealthCareApp.Shared.Dtos.Notifications
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }

        public int? PatientId { get; set; }

        public int? DoctorId { get; set; }

        public int? AppointmentId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string NotificationType { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public string CreatedDate { get; set; } = string.Empty;
    }
}