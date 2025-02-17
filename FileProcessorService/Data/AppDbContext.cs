using FileProcessorService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileProcessorService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) {}
    
    public DbSet<FileLogModel> FileLogs { get; set; }
}