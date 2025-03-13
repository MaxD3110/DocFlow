using FileManagementService.Models;

namespace FileManagementService.Data;

public interface IRepository<T> where T : BaseModel
{
    IQueryable<T> Query();

    Task<bool> SaveChangesAsync();

    Task<T?> GetByIdAsync(int id);

    Task<T> CreateAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task DeleteByIdAsync(int id);
}