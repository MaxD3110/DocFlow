using FileManagementService;

namespace FileProcessorService.DTOs;

public class FileToConvertDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string StoragePath { get; set; }

    public int ConvertToExtensionId { get; set; }

    public ConversionType Event { get; set; }
}