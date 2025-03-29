using FileProcessorService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileProcessorService.Data;

public class ExtensionRepository : IExtensionRepository
{
    private readonly AppDbContext _ctx;

    public ExtensionRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task SaveAsync(Extension model) => await _ctx.Extensions.AddAsync(model);

    public async Task<List<Extension>> GetAllAsync() => await _ctx.Extensions.ToListAsync();
    public async Task<Extension?> GetById(int id) => await _ctx.Extensions.FirstOrDefaultAsync(i => i.ExternalId == id);

    public async Task<bool> SaveChangesAsync() => await _ctx.SaveChangesAsync() >= 0;
}