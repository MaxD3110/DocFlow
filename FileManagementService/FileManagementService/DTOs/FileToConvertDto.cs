namespace FileManagementService.DTOs;

public class FileToConvertDto : BaseDto
{
    public string StoragePath { get; set; }
    
    public int ConvertToExtensionId { get; set; }

    public ConversionType Event { get; set; }
}