namespace HealthAxisCore_Api.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public int DoctorId { get; set; }

        public int AppointmentId { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}