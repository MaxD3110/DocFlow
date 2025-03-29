using FileProcessorService.DTOs;

namespace FileProcessorService.AsyncDataServices;

public class EventBus(IConfiguration config) : EventBusBase(config,
    config["RabbitMQ:SendExchange"],
    config["RabbitMQ:ListenExchange"]),
    IEventBus
{
    public Task SendConversionResult(FileConvertedDto file) => Publish(file);

    public Task ListenForConvertationRequests(Func<FileToConvertDto, Task> handler) => Subscribe(handler);
}