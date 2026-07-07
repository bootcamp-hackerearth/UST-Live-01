using MassTransit;

namespace HealthCareApp.Messaging.Test
{
    public class TestRabbitMqConsumer : IConsumer<TestRabbitMqMessage>
    {
        private readonly ILogger<TestRabbitMqConsumer> logger;

        public TestRabbitMqConsumer(ILogger<TestRabbitMqConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<TestRabbitMqMessage> context)
        {
            logger.LogInformation(
                """
                ------------------------------------------------------------
                | HEALTHAXIS RABBITMQ TEST MESSAGE CONSUMED               |
                | Message Id : {MessageId}                                 |
                | Message    : {Message}                                   |
                | Created At : {CreatedAt}                                 |
                ------------------------------------------------------------
                """,
                context.Message.MessageId,
                context.Message.Message,
                context.Message.CreatedAt);

            return Task.CompletedTask;
        }
    }
}