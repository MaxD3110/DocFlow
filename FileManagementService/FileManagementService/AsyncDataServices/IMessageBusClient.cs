using FileManagementService.DTOs;

namespace FileManagementService.AsyncDataServices;

public interface IMessageBusClient
{
    Task ConvertFile(FileToConvertDto file);
}