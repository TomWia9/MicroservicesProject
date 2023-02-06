using System.Text;
using CommandsService.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private readonly ILogger<MessageBusSubscriber> _logger;
    private IConnection? _connection;
    private IModel? _channel;
    private string? _queueName;

    public MessageBusSubscriber(IConfiguration configuration,
        IEventProcessor eventProcessor,
        ILogger<MessageBusSubscriber> logger)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;
        _logger = logger;

        InitializeRabbitMq();
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (moduleHandle, ea) =>
        {
            _logger.LogInformation("Event received!");

            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            _eventProcessor.ProcessEvent(message);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    private void InitializeRabbitMq()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"] ?? throw new InvalidOperationException())
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare();
            _channel.QueueBind(queue: _queueName,
                exchange: "trigger",
                routingKey: "");
            
            _logger.LogInformation("Listening on the Message Bus...");
            
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
        }
        catch (Exception e)
        {
            _logger.LogError("Could not connect to the Message Bus {EMessage}", e.Message);
        }
    }
    
    private void RabbitMQ_ConnectionShutDown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogInformation("RabbitMQ Connection shutdown");
    }

    public override void Dispose()
    {
        if (_channel is { IsOpen: false }) return;
        _channel?.Close();
        _connection?.Close();
        
        _logger.LogInformation("Message Bus disposed");
    }
}