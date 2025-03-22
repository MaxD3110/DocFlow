using Microsoft.AspNetCore.Mvc;

namespace FileManagementService.Controllers;

[ApiController]
[Route("api")]
public class CommonController : ControllerBase
{
    [HttpGet]
    [Route("status")]
    public IActionResult CheckServiceConnection()
    {
        Console.WriteLine("-- Testing file management service connection");
        return Ok("Connection established");
    }
}