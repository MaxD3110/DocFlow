namespace FileManagementService.AsyncDataServices;

public class EventBusBackground : BackgroundService
{
    private readonly IEventBus _eventBus;

    public EventBusBackground(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _eventBus.ListenForConvertedFiles(async convertedFile =>
        {
            Console.WriteLine($"--> Rabbit: Received converted file event for FileId: {convertedFile.OriginalFileId}");
        });
    }
}