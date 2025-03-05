namespace FileManagementService.DTOs;

public class FileDto
{
    public int Id { get; set; }
    
    public string? FileName { get; set; }
    
    public string? FileType { get; set; }

    public long FileSize { get; set; }

    public DateTime UploadedAt { get; set; }

    public string? StoragePath { get; set; }

    public bool IsConverted { get; set; }
}