namespace FileManagementService.DTOs;

public class ExtensionDto : BaseDto
{
    public string ExtensionName { get; set; }
    
    public string? Format { get; set; }
}