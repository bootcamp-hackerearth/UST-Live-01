namespace HealthApp.Api.Messaging.Events
{
    public class HeartbeatEvent
    {
        public string HeartbeatId { get; set; } = Guid.NewGuid().ToString();

        public string ServiceName { get; set; } = "HealthApp.Api";

        public string EventName { get; set; } = string.Empty;

        public string Status { get; set; } = "Success";

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}