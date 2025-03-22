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

        Console.WriteLine("DB is ready");
    }
}