using FileManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileManagementService.Data;

public class Repository<T> : IRepository<T> where T : BaseModel
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public IQueryable<T> Query() => _context.Set<T>();

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;

    public Task<T?> GetByIdAsync(int id) => _dbSet.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);

        ArgumentNullException.ThrowIfNull(entity);

        await DeleteAsync(entity);
    }
}