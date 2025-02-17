using FileProcessorService.Data;
using FileProcessorService.Models;

namespace FileProcessorService.Services;

public class ProcessingService : IProcessingService
{
    private readonly IFileLogRepository _fileLogRepository;

    public ProcessingService(IFileLogRepository fileLogRepository)
    {
        _fileLogRepository = fileLogRepository;
    }
    
    public async Task<byte[]> ConvertToPdfAsync(byte[] file)
    {
        await Task.Delay(1000); // Имитация обработки
        await _fileLogRepository.SaveFileLogAsync(new FileLogModel());

        return file;
    }

    public async Task<byte[]> ConvertToPngAsync(byte[] file)
    {
        await Task.Delay(1000); // Имитация обработки
        await _fileLogRepository.SaveFileLogAsync(new FileLogModel());
        return file;
    }
}