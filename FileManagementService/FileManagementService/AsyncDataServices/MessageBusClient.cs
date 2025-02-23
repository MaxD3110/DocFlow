using System.Text;
using System.Text.Json;
using FileManagementService.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FileManagementService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel _channel;
    private readonly string _exchange;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        _exchange = configuration["RabbitMQ:Exchange"] ?? string.Empty;
        _ = InitializeAsync();
    }
    
    private async Task InitializeAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"] ?? string.Empty,
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? string.Empty)
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(exchange: _exchange, type: ExchangeType.Fanout);

        _connection.ConnectionShutdownAsync += ConnectionShutdown;
 
        Console.WriteLine("Message bus connection established");
    }

    private static Task ConnectionShutdown(object sender, ShutdownEventArgs @event)
    {
        Console.WriteLine("Message bus connection shutdown");
        return Task.CompletedTask;
    }

    private async Task SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        await _channel.BasicPublishAsync(
            exchange: _exchange,
            routingKey: string.Empty,
            body: body
        );

        Console.WriteLine($"Message sent {message}");
    }

    public async Task Dispose()
    {
        Console.WriteLine("Message bus disposed");

        if (_channel.IsOpen)
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }

    public async Task ConvertFile(FileToConvertDto file)
    {
        var message = JsonSerializer.Serialize(file);

        if (_connection != null && _connection.IsOpen)
        {
            Console.WriteLine("Sending message");
            await SendMessage(message);
        }
        else
            Console.WriteLine("Connection isn't opened");
    }
}