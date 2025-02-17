using FileProcessorService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileProcessorService.Controllers;

[Route("api/processor/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IProcessingService _processingService;

    public FilesController(IProcessingService processingService)
    {
        _processingService = processingService;
    }
    
    [HttpGet]
    public IActionResult PingInboundConnection()
    {
        Console.WriteLine("-- Testing processor connection");
        return Ok("Connection established");
    }

    [HttpPost]
    public async Task<IActionResult> ConvertFile(IFormFile? file, [FromForm] string format)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is empty");
        
        byte[] fileBytes;

        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            fileBytes = ms.ToArray();
        }
        
        byte[] convertedFile = await _processingService.ConvertToPdfAsync(fileBytes);

        return File(convertedFile, "application/octet-stream", "converted." + format);
    }
}