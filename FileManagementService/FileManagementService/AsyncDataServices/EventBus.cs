using FileManagementService.DTOs;

namespace FileManagementService.AsyncDataServices;

public class EventBus(IConfiguration config) : EventBusBase(config,
    config["RabbitMQ:SendExchange"],
    config["RabbitMQ:ListenExchange"]),
    IEventBus
{
    public Task RequestFileConversion(FileToConvertDto file) => Publish(file);

    public Task ListenForConvertedFiles(Func<FileConvertedDto, Task> handler) => Subscribe(handler);
}