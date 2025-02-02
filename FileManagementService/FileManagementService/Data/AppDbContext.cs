using FileManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileManagementService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) {}
    
    public DbSet<FileModel> Files { get; set; }
}