using FileManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileManagementService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) {}
    
    public DbSet<FileModel> Files { get; set; }
    
    public DbSet<Extension> Extensions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Extension>()
            .HasMany(e => e.ConvertibleTo)
            .WithMany(e => e.ConvertibleFrom)
            .UsingEntity<Dictionary<string, object>>(
                "ExtensionConversion",
                j => j.HasOne<Extension>().WithMany().HasForeignKey("TargetExtensionId"),
                j => j.HasOne<Extension>().WithMany().HasForeignKey("SourceExtensionId"),
                j => j.HasKey("SourceExtensionId", "TargetExtensionId")
            );
    }
}