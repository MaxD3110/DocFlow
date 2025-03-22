using Microsoft.AspNetCore.Mvc;

namespace FileProcessorService.Controllers;

[ApiController]
[Route("api/processor")]
public class CommonController : ControllerBase
{
    [HttpGet]
    [Route("status")]
    public IActionResult CheckServiceConnection()
    {
        Console.WriteLine("-- Testing file processor service connection");
        return Ok("Connection established");
    }
}