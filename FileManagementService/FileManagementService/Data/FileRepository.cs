using FileManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileManagementService.Data;

public class FileRepository : IFileRepository
{
    private readonly AppDbContext _context;
    
    public FileRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<IEnumerable<FileModel>> GetFilesAsync()
    {
        return await _context.Files.ToListAsync();
    }

    public async Task<FileModel?> GetFileAsync(int id)
    {
        return await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateFileAsync(FileModel file)
    {
        ArgumentNullException.ThrowIfNull(file);

        await _context.Files.AddAsync(file);
    }
}