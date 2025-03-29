using FileProcessorService.EventProcessing;

namespace FileProcessorService.AsyncDataServices;

public class EventBusBackground : BackgroundService
{
    private readonly IEventBus _eventBus;
    private readonly IEventProcessor _eventProcessor;

    public EventBusBackground(IEventBus eventBus, IEventProcessor eventProcessor)
    {
        _eventBus = eventBus;
        _eventProcessor = eventProcessor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _eventBus.ListenForConvertationRequests(async convertedFile =>
        {
            await _eventProcessor.ProccessEventAsync(convertedFile);
            Console.WriteLine($"--> RabbitMQ: Received conversion file event for FileId: {convertedFile.Id}");
        });
    }
}