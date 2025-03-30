using FileManagementService.DTOs;

namespace FileManagementService.EventProcessing;

public interface IEventProcessor
{
    Task ProccessEventAsync(FileConvertedDto? file);
}