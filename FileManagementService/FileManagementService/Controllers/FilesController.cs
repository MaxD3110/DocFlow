using AutoMapper;
using FileManagementService.AsyncDataServices;
using FileManagementService.Data;
using FileManagementService.DTOs;
using FileManagementService.Models;
using FileManagementService.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileManagementService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IRepository<FileModel> _fileRepository;
    private readonly IRepository<Extension> _extensionRepository;
    private readonly IMapper _mapper;
    private readonly IProcessorDataClient _processorDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public FilesController(
        IRepository<FileModel> fileRepository,
        IRepository<Extension> extensionRepository,
        IMapper mapper,
        IProcessorDataClient processorDataClient,
        IMessageBusClient messageBusClient)
    {
        _fileRepository = fileRepository;
        _extensionRepository = extensionRepository;
        _mapper = mapper;
        _processorDataClient = processorDataClient;
        _messageBusClient = messageBusClient;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFiles()
    {
        var files = await _fileRepository.Query()
            .ToListAsync();
        var mappedFiles = _mapper.Map<IEnumerable<FileDto>>(files);
        
        return Ok(mappedFiles);
    }

    [HttpGet("{id:int}", Name = "GetFileById")]
    public async Task<IActionResult> GetFileById(int id)
    {
        var file = await _fileRepository.GetByIdAsync(id);
        
        if (file == null)
            return BadRequest();
        
        var mappedFile = _mapper.Map<FileDto>(file);

        return Ok(mappedFile);
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {        
        var fileType = file.FileName.Split(".")[^1];
        var fileName = $"File_{Guid.NewGuid()}.{fileType}";
        var filePath = Path.Combine("StoredFiles", fileName);

        Directory.CreateDirectory("StoredFiles");

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileDto = new FileDto
        {
            Name = fileName,
            FileSize = file.Length,
            FileType = fileType,
            StoragePath = filePath,
            UploadedAt = DateTime.UtcNow
        };

        var fileModel = _mapper.Map<FileModel>(fileDto);

        await _fileRepository.CreateAsync(fileModel);
        
        if (fileModel.Id == 0)
            return BadRequest("Не удалось сохранить файл");
        
        return Ok(fileModel.Id);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFile(FileDto file)
    {
        var fileModel = _mapper.Map<FileModel>(file);

        await _fileRepository.CreateAsync(fileModel);
        
        var fileModelToDto = _mapper.Map<FileDto>(fileModel);

        try
        {
            await _processorDataClient.SendFileToProcessorAsync(fileModelToDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not send file to processor: {e.Message}");
            throw;
        }

        try
        {
            var fileToConvert = _mapper.Map<FileToConvertDto>(fileModelToDto);

            fileToConvert.Event = "FileAwaitConvertation";

            await _messageBusClient.ConvertFile(fileToConvert);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not send async file to processor: {e.Message}");
            throw;
        }

        return CreatedAtRoute(nameof(GetFileById), new { id = fileModel.Id }, fileModelToDto);
    }
}