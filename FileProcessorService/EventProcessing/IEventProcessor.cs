using FileProcessorService.Models;

namespace FileProcessorService.EventProcessing;

public interface IEventProcessor
{
    Task ProccessEventAsync(string message);
}