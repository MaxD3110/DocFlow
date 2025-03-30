using FileManagementService.EventProcessing;

namespace FileManagementService.AsyncDataServices;

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
        await _eventBus.ListenForConvertedFiles(async convertedFile =>
        {
            await _eventProcessor.ProccessEventAsync(convertedFile);
            
            Console.WriteLine($"--> Rabbit: Received converted file event for FileId: {convertedFile.OriginalFileId}");
        });
    }
}