namespace FileManagementService.DTOs;

public class FileConvertedDto : BaseDto
{
    public int OriginalFileId { get; set; }

    public int TargetExtensionId { get; set; }
    
    public string StoragePath { get; set; }

    public long FileSize { get; set; }
}