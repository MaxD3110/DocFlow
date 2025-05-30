using FileProcessorService.DTOs;
using FileProcessorService.Models;

namespace FileProcessorService.EventProcessing;

public interface IEventProcessor
{
    Task ProcessEventAsync(FileToConvertDto? file);
}