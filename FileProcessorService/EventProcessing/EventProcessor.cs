using System.Text.Json;
using AutoMapper;
using FileProcessorService.Data;
using FileProcessorService.DTOs;
using FileProcessorService.Models;

namespace FileProcessorService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    
    public async Task ProccessEventAsync(string message)
    {
        var eventType = DetermineEventType(message);

        switch (eventType)
        {
            case EventType.FileAwaitConvertation:
                await ConvertFile(message);
                break;
            default:
                Console.WriteLine("Undetermined event type");
                break;
        }
    }

    private async Task ConvertFile(string fileToConvertMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<IFileLogRepository>();

            var fileToConvertDto = JsonSerializer.Deserialize<FileToConvertDto>(fileToConvertMessage);

            try
            {
                var log = _mapper.Map<FileLogModel>(fileToConvertDto);
                await repo.SaveFileLogAsync(log);
                await repo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to save file to log {e.Message}");
                throw;
            }
        }
    }

    private EventType DetermineEventType(string message)
    {
        Console.WriteLine("Determine Event");
        
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

        return eventType?.Event switch
        {
            "FileAwaitConvertation" => EventType.FileAwaitConvertation,
            _ => EventType.Undetermined
        };
    }

    enum EventType
    {
        FileAwaitConvertation,
        Undetermined
    }
}