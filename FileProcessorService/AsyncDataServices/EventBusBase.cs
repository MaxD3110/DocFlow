using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FileProcessorService.AsyncDataServices;

public abstract class EventBusBase
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string? _exchangePublish;
    private readonly string? _exchangeListen;

    protected EventBusBase(IConfiguration configuration, string? exchangePublishName = null, string? exchangeListenName = null)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? string.Empty,
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? string.Empty)
        };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;
        _exchangePublish = exchangePublishName;
        _exchangeListen = exchangeListenName;

        _connection.ConnectionShutdownAsync += ConnectionShutdown;

        Console.WriteLine("--> Rabbit: Connected to RabbitMQ");

        if (_exchangePublish == null) return;

        _channel.ExchangeDeclareAsync(_exchangePublish, ExchangeType.Fanout).Wait();
        Console.WriteLine($"--> Rabbit: Using exchange '{_exchangePublish}'");
        
        if (_exchangeListen == null) return;

        Console.WriteLine($"--> Rabbit: Using exchange '{_exchangeListen}'");
    }

    private static Task ConnectionShutdown(object sender, ShutdownEventArgs @event)
    {
        Console.WriteLine("--> Rabbit: Message bus connection shutdown");
        return Task.CompletedTask;
    }

    protected async Task Publish<T>(T message)
    {
        if (_exchangePublish == null)
        {
            Console.WriteLine("--> Rabbit: publish exchange not configured");
            return;
        }

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await _channel.BasicPublishAsync(
            exchange: _exchangePublish,
            routingKey: "",
            body: body
        );

        Console.WriteLine($"--> Rabbit: Published event to {_exchangePublish}: {typeof(T).Name}");
    }

    protected async Task Subscribe<T>(Func<T, Task> onMessage)
    {
        if (_exchangeListen == null)
        {
            Console.WriteLine("--> Rabbit: listen exchange not configured");
            return;
        }
        
        var queue = await _channel.QueueDeclareAsync();
        await _channel.QueueBindAsync(queue.QueueName, _exchangeListen, "");

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<T>(messageBody);

            if (message != null)
            {
                Console.WriteLine($"--> Rabbit: Received event from {_exchangeListen}: {typeof(T).Name}");
                await onMessage(message);
            }
        };

        await _channel.BasicConsumeAsync(queue.QueueName, true, consumer);
    }

    public async Task Dispose()
    {
        Console.WriteLine("--> Rabbit: Message bus disposed");

        if (_channel.IsOpen)
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }
}