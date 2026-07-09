namespace HealthApp.API.Messaging;

public interface IRabbitMQPublisher
{
    Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default);
}
