using System.Text;
using FileProcessorService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FileProcessorService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _exchange;
    private string? _queueName;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;
        _exchange = configuration["RabbitMQ:Exchange"] ?? string.Empty;
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

        var queue = await _channel.QueueDeclareAsync();
        _queueName = queue.QueueName;
        await _channel.QueueBindAsync(queue: _queueName,
            exchange: _exchange,
            routingKey: string.Empty);
        
        _connection.ConnectionShutdownAsync += ConnectionShutdown;

        Console.WriteLine("Message bus listening");
    }

    public override void Dispose()
    {
        Console.WriteLine("Message bus disposed");
        
        base.Dispose();
    }
    
    private static Task ConnectionShutdown(object sender, ShutdownEventArgs @event)
    {
        Console.WriteLine("Message bus connection shutdown");
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        await InitializeAsync();

        var consumer = new AsyncEventingBasicConsumer(_channel);
            
        consumer.ReceivedAsync += (moduleHandle, ea) =>
        {
            Console.WriteLine("Message received!");
                
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            _eventProcessor.ProccessEventAsync(notificationMessage);
                
            return Task.CompletedTask;
        };
            
        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer, cancellationToken: stoppingToken);
    }
}