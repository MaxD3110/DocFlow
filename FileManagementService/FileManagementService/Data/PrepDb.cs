using FileManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileManagementService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            SetupDatabase(scope.ServiceProvider.GetService<AppDbContext>());
        }
    }

    private static void SetupDatabase(AppDbContext context)
    {
        try
        {
            context.Database.Migrate();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        if (!context.Files.Any())
        {
            Console.WriteLine("Creating data... test");

            context.Files.AddRange(
                new FileModel {Name = "11111"},
                new FileModel {Name = "22222"},
                new FileModel {Name = "3333"},
                new FileModel {Name = "4444"});
            
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("already good");
        }
    }
}