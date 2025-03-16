using FileManagementService.AsyncDataServices;
using FileManagementService.Data;
using FileManagementService.SyncDataServices.Grpc;
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

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<IMessageBusClient, MessageBusClient>();

        services.AddGrpc();

        services.AddHttpClient<IProcessorDataClient, HttpProcessorDataClient>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGrpcService<GrpcFileManagementService>();
            endpoints.MapGet("/protos/filemanagement.proto", async context =>
            {
                await context.Response.WriteAsync(File.ReadAllText("protos/filemanagement.proto"));
            });
        });

        PrepDb.PrepPopulation(app);
    }
 
}