namespace FileProcessorService.Services;

public interface IProcessingService
{
    Task<byte[]> ConvertToPdfAsync (byte[] file);

    Task<byte[]> ConvertToPngAsync (byte[] file);
}