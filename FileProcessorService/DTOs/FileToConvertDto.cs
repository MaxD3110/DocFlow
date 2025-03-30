namespace FileProcessorService.DTOs;

public class FileToConvertDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string StoragePath { get; set; }

    public int SourceExtensionId { get; set; }

    public int TargetExtensionId { get; set; }
    
    public bool SaveAsNew { get; set; }
}