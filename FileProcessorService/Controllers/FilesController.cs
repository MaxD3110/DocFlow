using Microsoft.AspNetCore.Mvc;

namespace FileProcessorService.Controllers;

[Route("api/processor/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    [HttpPost]
    public IActionResult TestConnection()
    {
        Console.WriteLine("Test processor Connection");
        return Ok("alr");
    }
}