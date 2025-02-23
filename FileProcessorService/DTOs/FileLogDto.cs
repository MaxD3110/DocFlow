namespace FileProcessorService.DTOs;

public class FileLogDto
{
    public int Id { get; set;}

    public int ExternalId { get; set; }

    public string? FileName { get; set; }

    public string? InputFormat { get; set; }

    public string? OutputFormat { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Status { get; set; }
}