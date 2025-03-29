using FileProcessorService.Models;

namespace FileProcessorService.Data;

public interface IExtensionRepository
{
    Task SaveAsync(Extension model);

    Task<bool> SaveChangesAsync();
    
    Task<List<Extension>> GetAllAsync();

    Task<Extension?> GetById(int id);
}