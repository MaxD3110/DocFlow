using FileManagementService.DTOs;

namespace FileManagementService.SyncDataServices.Http;

public interface IProcessorDataClient
{
    Task SendFileToProcessorAsync(FileDto file);
}