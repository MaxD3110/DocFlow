using FileProcessorService.Data;
using FileProcessorService.Services;
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
        services.AddScoped<IProcessingService, ProcessingService>();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
 
}