namespace FileProcessorService.DTOs;

public class FileConvertedDto
{
    public string Name { get; set; }

    public int OriginalFileId { get; set; }

    public int TargetExtensionId { get; set; }

    public string StoragePath { get; set; }

    public long FileSize { get; set; }
}