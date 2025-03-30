using FileProcessorService.DTOs;

namespace FileProcessorService.EventProcessing;

public interface IEventHelper
{
    public Task<MemoryStream?> ConvertFile(FileToConvertDto file);

    public Task<FileConvertedDto> TrySaveFile(FileToConvertDto file, MemoryStream stream);
    
    public Task AddToLog(FileToConvertDto file);
}