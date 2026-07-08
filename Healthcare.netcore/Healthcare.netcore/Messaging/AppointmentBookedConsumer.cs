using System.Text;
using System.Text.Json;
using HealthAxis.API.Data;
using HealthAxis.API.Events;
using HealthAxis.API.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HealthAxis.API.Messaging;

public sealed class AppointmentBookedConsumer : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;

    private readonly ILogger<AppointmentBookedConsumer> _logger;
    private readonly IConfiguration _config;
    private readonly IServiceScopeFactory _scopeFactory;

    public AppointmentBookedConsumer(
        ILogger<AppointmentBookedConsumer> logger,
        IConfiguration config,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _config = config;
        _scopeFactory = scopeFactory;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var rabbitConfig = _config.GetSection("RabbitMq");

        var factory = new ConnectionFactory
        {
            HostName = rabbitConfig["HostName"]!,
            Port = int.Parse(rabbitConfig["Port"]!),
            UserName = rabbitConfig["UserName"]!,
            Password = rabbitConfig["Password"]!,
            VirtualHost = rabbitConfig["VirtualHost"]!
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        _logger.LogInformation("AppointmentBookedConsumer started in background.");

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel == null)
        {
            _logger.LogError("RabbitMQ channel is not initialized.");
            return;
        }

        var queueName = _config.GetSection("RabbitMq")["AppointmentQueue"];

        await _channel.QueueDeclareAsync(
            queue: queueName!,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var appointmentEvent = JsonSerializer.Deserialize<AppointmentBookedEvent>(message);

                if (appointmentEvent == null)
                {
                    _logger.LogWarning("Received empty AppointmentBookedEvent message.");
                    await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                    return;
                }

                _logger.LogInformation(
                    "Consumed AppointmentBookedEvent: AppointmentId={AppointmentId}, Patient={PatientName}, DoctorId={DoctorId}, Date={Date}, Slot={TimeSlot}",
                    appointmentEvent.AppointmentId,
                    appointmentEvent.PatientName,
                    appointmentEvent.DoctorId,
                    appointmentEvent.ScheduledDate,
                    appointmentEvent.TimeSlot);

                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthAxisDbContext>();

                var notification = new Notification
                {
                    DoctorId = appointmentEvent.DoctorId,
                    Message = $"New appointment booked by {appointmentEvent.PatientName} on {appointmentEvent.ScheduledDate:dd-MMM-yyyy} at {appointmentEvent.TimeSlot}.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                await dbContext.Notifications.AddAsync(notification, stoppingToken);
                await dbContext.SaveChangesAsync(stoppingToken);

                _logger.LogInformation(
                    "Notification created for DoctorId {DoctorId} for AppointmentId {AppointmentId}",
                    appointmentEvent.DoctorId,
                    appointmentEvent.AppointmentId);

                await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while consuming AppointmentBookedEvent.");

                await _channel.BasicNackAsync(
                    eventArgs.DeliveryTag,
                    multiple: false,
                    requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: queueName!,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("AppointmentBookedConsumer cancellation requested.");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("AppointmentBookedConsumer stopping.");

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