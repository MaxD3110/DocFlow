using AutoMapper;
using FileManagementService;
using FileProcessorService.Models;
using Grpc.Net.Client;

namespace FileProcessorService.SyncDataServices.Grpc;

public class FileManagementDataClient : IFileManagementDataClient
{
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public FileManagementDataClient(IConfiguration config, IMapper mapper)
    {
        _config = config;
        _mapper = mapper;
    }

    public IEnumerable<Extension> GetExtensions()
    {
        Console.WriteLine($"Calling GRPC service {_config["GrpcFileManagement"]}");

        var channel = GrpcChannel.ForAddress(_config["GrpcFileManagement"]);
        var client = new GrpcFileExtension.GrpcFileExtensionClient(channel);
        var request = new GetAllFileExtensionsRequest();

        try
        {
            var reply = client.GetAllFileExtensions(request);
            
            return _mapper.Map<IEnumerable<Extension>>(reply.Extension);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Grpc fail: {e.Message}");
            throw;
        }
    }
}