using FileManagementService.Models;

namespace FileManagementService.Data;

public interface IFileRepository
{
    Task<bool> SaveChangesAsync();

    Task<IEnumerable<FileModel>> GetFilesAsync();
    
    Task<FileModel?> GetFileAsync(int id);
    
    Task CreateFileAsync(FileModel file);
}