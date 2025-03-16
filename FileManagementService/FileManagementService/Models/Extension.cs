namespace FileManagementService.Models;

public class Extension : BaseModel
{
    public string? MediaType { get; set; }

    // Extension format e.g. .pdf, .jpg, .docx
    public required string FilenameExtension { get; set; }
    
    // Extensions that this extension can be converted to
    public List<Extension> ConvertibleTo { get; set; } = [];

    // Extensions that can be converted to this extension
    public List<Extension> ConvertibleFrom { get; set; } = [];
}