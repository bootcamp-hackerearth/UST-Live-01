using HealthAxisCore_Api.Messaging.Contracts;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;
using System.Text.Json;

namespace HealthAxisCore_Api.Messaging.Consumers
{
    public class AppointmentBookedConsumer : BackgroundService
    {
        private static readonly Serilog.ILogger Logger =
            Log.ForContext<AppointmentBookedConsumer>();

        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private IConnection? _connection;
        private IChannel? _channel;

        public AppointmentBookedConsumer(
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var rabbitConfig = _configuration.GetSection("RabbitMq");

            var factory = new ConnectionFactory
            {
                HostName = rabbitConfig["HostName"]!,
                Port = int.Parse(rabbitConfig["Port"]!),
                UserName = rabbitConfig["UserName"]!,
                Password = rabbitConfig["Password"]!,
                VirtualHost = rabbitConfig["VirtualHost"]!
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);

            _channel = await _connection.CreateChannelAsync(
                cancellationToken: cancellationToken);

            Logger.Information(
                "AppointmentBookedConsumer started. Listening to RabbitMQ queue...");

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueName = _configuration
                .GetSection("RabbitMq")["AppointmentBookedQueue"]!;

            await _channel!.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                try
                {
                    var body = eventArgs.Body.ToArray();

                    var message = Encoding.UTF8.GetString(body);

                    var appointmentBookedEvent =
                        JsonSerializer.Deserialize<AppointmentBookedEvent>(
                            message,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                    if (appointmentBookedEvent == null)
                    {
                        Logger.Warning(
                            "RabbitMQ message could not be deserialized. Queue: {QueueName}",
                            queueName);

                        await _channel.BasicAckAsync(
                            eventArgs.DeliveryTag,
                            multiple: false,
                            cancellationToken: stoppingToken);

                        return;
                    }

                    Logger.Information(
                        "AppointmentBookedEvent consumed. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
                        appointmentBookedEvent.AppointmentId,
                        appointmentBookedEvent.DoctorId);

                    using var scope = _serviceScopeFactory.CreateScope();

                    var notificationRepository =
                        scope.ServiceProvider.GetRequiredService<IRepository<Notification>>();

                    var notification = new Notification
                    {
                        DoctorId = appointmentBookedEvent.DoctorId,
                        Message =
                            $"New appointment booked by {appointmentBookedEvent.PatientName} on " +
                            $"{appointmentBookedEvent.ScheduledDate:yyyy-MM-dd} at {appointmentBookedEvent.TimeSlot}.",
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    };

                    await notificationRepository.CreateAsync(
                        notification,
                        stoppingToken);

                    Logger.Information(
                        "Doctor notification created. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
                        appointmentBookedEvent.AppointmentId,
                        appointmentBookedEvent.DoctorId);

                    await _channel.BasicAckAsync(
                        eventArgs.DeliveryTag,
                        multiple: false,
                        cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.Error(
                        ex,
                        "Error while processing AppointmentBookedEvent from RabbitMQ.");

                    await _channel!.BasicNackAsync(
                        eventArgs.DeliveryTag,
                        multiple: false,
                        requeue: true,
                        cancellationToken: stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.Information("AppointmentBookedConsumer stopping...");

            if (_channel != null)
            {
                await _channel.CloseAsync(cancellationToken);
            }

            if (_connection != null)
            {
                await _connection.CloseAsync(cancellationToken);
            }

            await base.StopAsync(cancellationToken);
        }
    }
}