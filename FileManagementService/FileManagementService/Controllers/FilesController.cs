using AutoMapper;
using FileManagementService.AsyncDataServices;
using FileManagementService.Data;
using FileManagementService.DTOs;
using FileManagementService.Models;
using FileManagementService.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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
    private readonly IEventBus _eventBusClient;
    private readonly IConfiguration _configuration;

    public FilesController(
        IRepository<FileModel> fileRepository,
        IRepository<Extension> extensionRepository,
        IMapper mapper,
        IProcessorDataClient processorDataClient,
        IEventBus eventBusClient,
        IConfiguration configuration)
    {
        _fileRepository = fileRepository;
        _extensionRepository = extensionRepository;
        _mapper = mapper;
        _processorDataClient = processorDataClient;
        _eventBusClient = eventBusClient;
        _configuration = configuration;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFiles()
    {
        var files = await _fileRepository.Query()
            .Include(f => f.Extension)
            .ThenInclude(e => e!.ConvertibleTo)
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
        var filenameExtension = Path.GetExtension(file.FileName).TrimStart('.');
        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(file.FileName, out string contentType))
            contentType = "application/octet-stream"; // Fallback if unknown

        var existingExtension = await _extensionRepository.Query()
            .FirstOrDefaultAsync(e => e.MediaType == contentType);

        if (existingExtension == null)
        {
            var newExtension = new Extension
            {
                Name = filenameExtension.ToUpper(),
                FilenameExtension = filenameExtension,
                MediaType = contentType
            };

            existingExtension = await _extensionRepository.CreateAsync(newExtension);
        }

        var storagePath = _configuration["StoredFilesPath"] ?? string.Empty;
        var filePath = Path.Combine(storagePath, file.FileName);

        Directory.CreateDirectory(storagePath);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileModel = new FileModel
        {
            Name = file.FileName,
            Extension = existingExtension,
            FileSize = file.Length,
            StoragePath = filePath,
            UploadedAt = DateTime.UtcNow
        };

        await _fileRepository.CreateAsync(fileModel);
        
        if (fileModel.Id == 0)
            return BadRequest("Failed to save file");
        
        return Ok(fileModel.Id);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFile(int id)
    {
        var file = await _fileRepository.GetByIdAsync(id);
        
        if (file == null)
            return NotFound();
            
        try
        {
            if (!string.IsNullOrEmpty(file.StoragePath))
                System.IO.File.Delete(file.StoragePath);
            
            await _fileRepository.DeleteAsync(file);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost("bulkDelete")]
    public async Task<IActionResult> BulkDelete([FromBody] List<int> fileIds)
    {
        var files = await _fileRepository.Query()
            .Where(i => fileIds.Contains(i.Id))
            .ToListAsync();
        
        if (!files.Any())
            return NotFound();

        foreach (var file in files)
        {
            try
            {
                if (!string.IsNullOrEmpty(file.StoragePath))
                    System.IO.File.Delete(file.StoragePath);
            
                await _fileRepository.DeleteAsync(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        return Ok();
    }
    
    [HttpPost("convertFile/{fileId:int}-{extensionId:int}")]
    public async Task<IActionResult> ConvertFile(int fileId, int extensionId)
    {
        var file = await _fileRepository.GetByIdAsync(fileId);
        
        if (file == null)
            return NotFound("File not found");

        try
        {
            var fileToConvert = _mapper.Map<FileToConvertDto>(file);

            fileToConvert.ConvertToExtensionId = extensionId;
            fileToConvert.Event = ConversionType.PdfToDoc;

            await _eventBusClient.RequestFileConversion(fileToConvert);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not send async file to processor: {e.Message}");
            throw;
        }

        return Ok();
    }
}