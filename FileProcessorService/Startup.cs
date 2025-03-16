using FileProcessorService.AsyncDataServices;
using FileProcessorService.Data;
using FileProcessorService.EventProcessing;
using FileProcessorService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

namespace FileProcessorService;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("memoryDb"));

        services.AddScoped<IFileLogRepository, FileLogRepository>();
        services.AddScoped<IExtensionRepository, ExtensionRepository>();
        services.AddScoped<IFileManagementDataClient, FileManagementDataClient>();
        services.AddSingleton<IEventProcessor, EventProcessor>();

        services.AddHostedService<MessageBusSubscriber>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        PrepDb.PrepPopulation(app);
    }
 
}