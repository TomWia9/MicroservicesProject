using System.Text;
using System.Text.Json;
using PlatformService.Interfaces;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient<T> : IMessageBusClient<T> where T : class
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessageBusClient<T>> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public MessageBusClient(IConfiguration configuration, ILogger<MessageBusClient<T>> logger)
    {
        _configuration = configuration;
        _logger = logger;
        ConnectToRabbitMq();
    }
    
    public void Publish(T dto)
    {
        var message = JsonSerializer.Serialize(dto);

        if (_connection is { IsOpen: true })
        {
            _logger.LogInformation("RabbitMQ connection open, sending message...");
            SendMessage(message);
        }
        else
        {
            _logger.LogInformation("RabbitMQ connection closed, not sending");
        }
    }

    public void Dispose()
    {
        _logger.LogInformation("Message Bus disposed");

        if (_channel is { IsOpen: false }) return;
        _channel?.Close();
        _connection?.Close();
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
        
        _logger.LogInformation("Message {Message} has been sent", message);
    }

    private void ConnectToRabbitMq()
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

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
            _logger.LogInformation("Connected to Message Bus");
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
}