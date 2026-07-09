namespace HealthCareApp.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public int DoctorId { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public bool IsRead { get; set; }
    }
}