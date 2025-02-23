using FileManagementService.AsyncDataServices;
using FileManagementService.Data;
using FileManagementService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;

namespace FileManagementService;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("ManagementConnection")));

        services.AddScoped<IFileRepository, FileRepository>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();

        services.AddHttpClient<IProcessorDataClient, HttpProcessorDataClient>();

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