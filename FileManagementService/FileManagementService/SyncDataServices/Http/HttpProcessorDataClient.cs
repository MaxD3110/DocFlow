using System.Text;
using System.Text.Json;
using FileManagementService.DTOs;

namespace FileManagementService.SyncDataServices.Http;

public class HttpProcessorDataClient : IProcessorDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpProcessorDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    public async Task SendFileToProcessorAsync(FileDto file)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(file),
            Encoding.UTF8,
            "application/json");
        
        var response = await _httpClient.PostAsync(_configuration["ProcessorService"], httpContent);
        
        if (response.IsSuccessStatusCode)
            Console.WriteLine("-- Processor service connection established");
        else
            Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}