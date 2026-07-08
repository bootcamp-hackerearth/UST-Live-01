using HealthAxisCore_Api.Messaging.Contracts;
using RabbitMQ.Client;
using Serilog;
using System.Text;
using System.Text.Json;

namespace HealthAxisCore_Api.Messaging.Publishers
{
    public class RabbitMQPublisher : IDisposable
    {
        private static readonly Serilog.ILogger Logger =
            Log.ForContext<RabbitMQPublisher>();

        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly string _queueName;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            var rabbitConfig = configuration.GetSection("RabbitMq");

            var factory = new ConnectionFactory
            {
                HostName = rabbitConfig["HostName"]!,
                Port = int.Parse(rabbitConfig["Port"]!),
                UserName = rabbitConfig["UserName"]!,
                Password = rabbitConfig["Password"]!,
                VirtualHost = rabbitConfig["VirtualHost"]!
            };

            _queueName = rabbitConfig["AppointmentBookedQueue"]!;

            _connection = factory
                .CreateConnectionAsync()
                .GetAwaiter()
                .GetResult();

            _channel = _connection
                .CreateChannelAsync()
                .GetAwaiter()
                .GetResult();

            _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null)
                .GetAwaiter()
                .GetResult();

            Logger.Information(
                "RabbitMQ publisher initialized. Queue: {QueueName}",
                _queueName);
        }

        public async Task PublishAppointmentBookedAsync(
            AppointmentBookedEvent appointmentBookedEvent,
            CancellationToken ct = default)
        {
            var message = JsonSerializer.Serialize(appointmentBookedEvent);

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
                body: body,
                cancellationToken: ct);

            Logger.Information(
                "AppointmentBookedEvent published to RabbitMQ. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}, Queue: {QueueName}",
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId,
                _queueName);
        }

        public void Dispose()
        {
            _channel.CloseAsync().GetAwaiter().GetResult();
            _connection.CloseAsync().GetAwaiter().GetResult();
        }
    }
}