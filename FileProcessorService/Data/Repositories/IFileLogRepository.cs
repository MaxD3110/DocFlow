using FileProcessorService.Models;

namespace FileProcessorService.Data;

public interface IFileLogRepository
{
    Task SaveFileLogAsync(FileLogModel model);

    Task<bool> SaveChangesAsync();
    
    Task<List<FileLogModel>> GetFileLogsAsync();
}