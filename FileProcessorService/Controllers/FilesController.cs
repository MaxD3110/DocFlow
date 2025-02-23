using AutoMapper;
using FileProcessorService.Data;
using FileProcessorService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FileProcessorService.Controllers;

[Route("api/processor/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFileLogRepository _fileLogRepository;
    private readonly IMapper _mapper;

    public FilesController(IFileLogRepository fileLogRepository, IMapper mapper)
    {
        _fileLogRepository = fileLogRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("Ping")]
    public IActionResult PingInboundConnection()
    {
        Console.WriteLine("-- Testing processor connection");
        return Ok("Connection established");
    }
    
    [HttpGet]
    [Route("GetLogs")]
    public async Task<IActionResult> GetLogs()
    {
        var fileLogs = await _fileLogRepository.GetFileLogsAsync();

        var result = _mapper.Map<IEnumerable<FileLogDto>>(fileLogs);
        
        return Ok(result);
    }

    [HttpPost]
    public IActionResult ConvertFile(IFormFile? file, [FromForm] string format)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is empty");

        return Ok();
    }
}