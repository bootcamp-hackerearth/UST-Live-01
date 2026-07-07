using HealthCareApp.Messaging.Test;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApp.Controllers
{
    [ApiController]
    [Route("api/messaging-test")]
    public class MessagingTestController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;

        private readonly ILogger<MessagingTestController> logger;

        public MessagingTestController(
            IPublishEndpoint publishEndpoint,
            ILogger<MessagingTestController> logger)
        {
            this.publishEndpoint = publishEndpoint;
            this.logger = logger;
        }

        [HttpPost("rabbitmq")]
        public async Task<IActionResult> PublishTestMessage()
        {
            var message = new TestRabbitMqMessage
            {
                MessageId = Guid.NewGuid(),
                Message = "HealthAxis RabbitMQ connectivity test message",
                CreatedAt = DateTime.UtcNow
            };

            await publishEndpoint.Publish(message);

            logger.LogInformation(
                """
                ------------------------------------------------------------
                | HEALTHAXIS RABBITMQ TEST MESSAGE PUBLISHED              |
                | Message Id : {MessageId}                                 |
                | Message    : {Message}                                   |
                | Created At : {CreatedAt}                                 |
                ------------------------------------------------------------
                """,
                message.MessageId,
                message.Message,
                message.CreatedAt);

            return Ok(new
            {
                Message = "RabbitMQ test message published successfully.",
                message.MessageId
            });
        }
    }
}