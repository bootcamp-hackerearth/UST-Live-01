namespace HealthAxis.Shared.DTO.NotificationDtos
{
    public sealed class NotificationDto
    {
        public int NotificationId { get; set; }

        public int? AppointmentId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string NotificationType { get; set; } =
            string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? DoctorName { get; set; }

        public string? DoctorSpecialisation { get; set; }
    }
}