using System.Text;
using System.Text.Json;
using HealthApp.API.Data;
using HealthApp.API.Events;
using HealthApp.API.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HealthApp.API.Messaging;

public class AppointmentBookedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AppointmentBookedConsumer> _logger;

    private IConnection? _connection;
    private IChannel? _channel;
    private string _queueName = string.Empty;

    public AppointmentBookedConsumer(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<AppointmentBookedConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await InitializeRabbitMqAsync(stoppingToken);

            if (_channel is null)
            {
                _logger.LogError("RabbitMQ consumer channel was not initialized.");
                return;
            }

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (_, eventArgs) =>
            {
                await HandleMessageAsync(eventArgs, stoppingToken);
            };

            await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            _logger.LogInformation(
                "AppointmentBookedConsumer started. Listening on queue: {QueueName}",
                _queueName);

            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("AppointmentBookedConsumer stopped because application shutdown was requested.");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "AppointmentBookedConsumer failed to start. Check RabbitMQ service and configuration.");
        }
    }

    private async Task InitializeRabbitMqAsync(CancellationToken stoppingToken)
    {
        var rabbitConfig = _configuration.GetSection("RabbitMq");

        _queueName = rabbitConfig["AppointmentQueue"]
            ?? throw new InvalidOperationException("RabbitMq:AppointmentQueue is missing.");

        var factory = new ConnectionFactory
        {
            HostName = rabbitConfig["HostName"] ?? "localhost",
            Port = int.Parse(rabbitConfig["Port"] ?? "5672"),
            UserName = rabbitConfig["UserName"] ?? "guest",
            Password = rabbitConfig["Password"] ?? "guest",
            VirtualHost = rabbitConfig["VirtualHost"] ?? "/"
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);
    }

    private async Task HandleMessageAsync(
        BasicDeliverEventArgs eventArgs,
        CancellationToken stoppingToken)
    {
        if (_channel is null)
        {
            return;
        }

        try
        {
            var body = eventArgs.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var appointmentBookedEvent =
                JsonSerializer.Deserialize<AppointmentBookedEvent>(json);

            if (appointmentBookedEvent is null)
            {
                _logger.LogWarning("Invalid AppointmentBookedEvent received from RabbitMQ.");

                await _channel.BasicAckAsync(
                    deliveryTag: eventArgs.DeliveryTag,
                    multiple: false,
                    cancellationToken: stoppingToken);

                return;
            }

            await CreateDoctorNotificationAsync(appointmentBookedEvent, stoppingToken);

            await _channel.BasicAckAsync(
                deliveryTag: eventArgs.DeliveryTag,
                multiple: false,
                cancellationToken: stoppingToken);

            _logger.LogInformation(
                "AppointmentBookedEvent consumed. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Message processing cancelled during shutdown.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing AppointmentBookedEvent.");

            await _channel.BasicNackAsync(
                deliveryTag: eventArgs.DeliveryTag,
                multiple: false,
                requeue: true,
                cancellationToken: CancellationToken.None);
        }
    }

    private async Task CreateDoctorNotificationAsync(
        AppointmentBookedEvent appointmentBookedEvent,
        CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<HealthAppDbContext>();

        var notification = new Notification
        {
            DoctorId = appointmentBookedEvent.DoctorId,
            AppointmentId = appointmentBookedEvent.AppointmentId,
            Title = "New Appointment Booked",
            Message =
                $"{appointmentBookedEvent.PatientName} booked an appointment on " +
                $"{appointmentBookedEvent.ScheduledDate:yyyy-MM-dd} at {appointmentBookedEvent.TimeSlot}.",
            IsRead = false,
            CreatedDate = DateTime.Now
        };

        await dbContext.Notifications.AddAsync(notification, stoppingToken);
        await dbContext.SaveChangesAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("AppointmentBookedConsumer shutdown requested.");

        if (_channel is not null)
        {
            await _channel.CloseAsync(cancellationToken);
            await _channel.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync(cancellationToken);
            await _connection.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}
