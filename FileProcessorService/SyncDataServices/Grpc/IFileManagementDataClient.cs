using FileProcessorService.Models;

namespace FileProcessorService.SyncDataServices.Grpc;

public interface IFileManagementDataClient
{
    IEnumerable<Extension> GetExtensions();
}