namespace FileManagementService.DTOs;

public class ExtensionDto : BaseDto
{
    public string MediaType { get; set; }
    
    public string? FilenameExtension { get; set; }
    
    public List<BaseDto> ConvertibleTo { get; set; } = [];
}