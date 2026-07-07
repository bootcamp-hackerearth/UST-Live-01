namespace HealthCareApp.Messaging.Test
{
    public class TestRabbitMqMessage
    {
        public Guid MessageId { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}