using FileProcessorService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileProcessorService.Data;

public class FileLogRepository : IFileLogRepository
{
    private readonly AppDbContext _ctx;

    public FileLogRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task SaveFileLogAsync(FileLogModel model) => await _ctx.FileLogs.AddAsync(model);

    public async Task<List<FileLogModel>> GetFileLogsAsync() => await _ctx.FileLogs.ToListAsync();

    public async Task<bool> SaveChangesAsync() => await _ctx.SaveChangesAsync() >= 0;
}