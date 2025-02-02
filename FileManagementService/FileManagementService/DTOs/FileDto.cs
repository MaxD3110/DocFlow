namespace FileManagementService.DTOs;

public class FileDto
{
    public int Id { get; set; }
    
    public string? Name { get; set; }

    public byte[]? Content { get; set; }

    public int FileSize { get; set; }

    public DateTime CreatedOn { get; set; }
}