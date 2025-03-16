using System.ComponentModel.DataAnnotations;

namespace FileProcessorService.Models;

public class Extension
{
    [Key]
    public int Id { get; set;}

    public int ExternalId { get; set;}

    public string? Name { get; set; }

    public string? FilenameExtension { get; set; }

    public string? MediaType { get; set; }
}