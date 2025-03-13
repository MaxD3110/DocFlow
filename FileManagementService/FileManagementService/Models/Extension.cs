namespace FileManagementService.Models;

public class Extension : BaseModel
{
    public string ExtensionName { get; set; }

    public string? Format { get; set; }
}