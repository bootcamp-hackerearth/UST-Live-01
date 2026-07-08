using System.Text;
using System.Text.Json;
using HealthAxis.API.Events;
using RabbitMQ.Client;

namespace HealthAxis.API.Messaging;

public sealed class RabbitMqPublisher : IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _queueName;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IConfiguration config, ILogger<RabbitMqPublisher> logger)
    {
        _logger = logger;

        var rabbitConfig = config.GetSection("RabbitMq");

        var factory = new ConnectionFactory
        {
            HostName = rabbitConfig["HostName"]!,
            Port = int.Parse(rabbitConfig["Port"]!),
            UserName = rabbitConfig["UserName"]!,
            Password = rabbitConfig["Password"]!,
            VirtualHost = rabbitConfig["VirtualHost"]!
        };

        _queueName = rabbitConfig["AppointmentQueue"]!;

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null).GetAwaiter().GetResult();
    }

    public async Task PublishAppointmentBookedAsync(AppointmentBookedEvent appointmentEvent)
    {
        var message = JsonSerializer.Serialize(appointmentEvent);
        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _queueName,
            mandatory: false,
            basicProperties: properties,
            body: body);

        _logger.LogInformation(
            "Published AppointmentBookedEvent for AppointmentId {AppointmentId}, DoctorId {DoctorId}",
            appointmentEvent.AppointmentId,
            appointmentEvent.DoctorId);
    }

    public void Dispose()
    {
        _channel.CloseAsync().GetAwaiter().GetResult();
        _connection.CloseAsync().GetAwaiter().GetResult();
    }
}