using FileProcessorService.DTOs;

namespace FileProcessorService.AsyncDataServices;

public interface IEventBus
{
    public Task SendConversionResult(FileConvertedDto file);

    public Task ListenForConvertationRequests(Func<FileToConvertDto, Task> handler);
}