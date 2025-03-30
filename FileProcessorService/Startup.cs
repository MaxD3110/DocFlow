using FileProcessorService.AsyncDataServices;
using FileProcessorService.Data;
using FileProcessorService.EventProcessing;
using FileProcessorService.FileConversion;
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
        services.AddScoped<IEventHelper, EventHelper>();
        
        // Event bus
        services.AddSingleton<IEventBus, EventBus>();
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddHostedService<EventBusBackground>();

        // Strategy factory
        services.Scan(scan => scan
            .FromAssemblyOf<IFileConverterStrategy>()
            .AddClasses(classes => classes.AssignableTo<IFileConverterStrategy>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
        services.AddSingleton<IFileConverterFactory, FileConverterFactory>();

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