namespace FileManagementService.DTOs;

public class FileConvertedDto : BaseDto
{
    public int OriginalFileId { get; set; }

    public string StoragePath { get; set; }

    public long FileSize { get; set; }

    public DateTime UploadedAt { get; set; }

    public ConversionType Event { get; set; }

    public ExtensionDto? Extension { get; set; }
}