using FileManagementService.DTOs;

namespace FileManagementService.AsyncDataServices;

public interface IEventBus
{
    public Task RequestFileConversion(FileToConvertDto file);

    public Task ListenForConvertedFiles(Func<FileConvertedDto, Task> handler);
}