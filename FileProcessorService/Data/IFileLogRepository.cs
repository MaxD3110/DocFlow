using FileProcessorService.Models;

namespace FileProcessorService.Data;

public interface IFileLogRepository
{
    Task SaveFileLogAsync(FileLogModel model);

    Task<bool> SaveChangesAsync(FileLogModel model);
    
    Task<List<FileLogModel>> GetFileLogsAsync();
}