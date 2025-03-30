namespace FileManagementService.DTOs;

public class FileToConvertDto : BaseDto
{
    public string StoragePath { get; set; }

    public int SourceExtensionId { get; set; }
    
    public int TargetExtensionId { get; set; }

    public bool SaveAsNew { get; set; }
}