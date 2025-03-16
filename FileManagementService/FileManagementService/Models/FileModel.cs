namespace FileManagementService.Models;

public class FileModel : BaseModel
{
    public int? ExtensionId { get; set; }

    public Extension? Extension { get; set; }

    public long FileSize { get; set; }

    public DateTime UploadedAt { get; set; }

    public string? StoragePath { get; set; }
}