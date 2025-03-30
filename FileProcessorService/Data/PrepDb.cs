using FileProcessorService.Models;
using FileProcessorService.SyncDataServices.Grpc;

namespace FileProcessorService.Data;

public static class PrepDb
{
    public static async Task PrepPopulation(IApplicationBuilder builder)
    {
        using (var serviceScope = builder.ApplicationServices.CreateScope())
        {
            var grpcClient = serviceScope.ServiceProvider.GetService<IFileManagementDataClient>();
            var extensions = grpcClient.GetExtensions();
        
            await SeedData(serviceScope.ServiceProvider.GetService<IExtensionRepository>(), extensions);
        }
    }

    private static async Task SeedData(IExtensionRepository extensionRepository, IEnumerable<Extension> extensions)
    {
        Console.WriteLine("Seeding extensions...");

        await extensionRepository.SaveAllAsync(extensions);
    }
}