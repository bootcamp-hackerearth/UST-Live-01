using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace HealthApp.API.Messaging;

public sealed class RabbitMQPublisher : IRabbitMQPublisher, IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);

    private IConnection? _connection;
    private IChannel? _channel;
    private string _queueName = string.Empty;

    public RabbitMQPublisher(
        IConfiguration configuration,
        ILogger<RabbitMQPublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default)
    {
        await EnsureInitializedAsync(cancellationToken);

        if (_channel is null)
        {
            throw new InvalidOperationException("RabbitMQ channel is not initialized.");
        }

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _queueName,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);

        _logger.LogInformation(
            "Message published to RabbitMQ. Queue: {QueueName}, MessageType: {MessageType}",
            _queueName,
            typeof(TMessage).Name);
    }

    private async Task EnsureInitializedAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null && _channel.IsOpen)
        {
            return;
        }

        await _connectionLock.WaitAsync(cancellationToken);

        try
        {
            if (_channel is not null && _channel.IsOpen)
            {
                return;
            }

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

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            _logger.LogInformation(
                "RabbitMQ publisher initialized. Queue: {QueueName}",
                _queueName);
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        _connectionLock.Dispose();
    }
}
