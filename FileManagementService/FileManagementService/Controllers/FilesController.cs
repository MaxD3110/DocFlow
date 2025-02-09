using AutoMapper;
using FileManagementService.Data;
using FileManagementService.DTOs;
using FileManagementService.Models;
using FileManagementService.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileManagementService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFileRepository _fileRepository;
    private readonly IMapper _mapper;
    private readonly IProcessorDataClient _processorDataClient;

    public FilesController(
        IFileRepository fileRepository,
        IMapper mapper,
        IProcessorDataClient processorDataClient)
    {
        _fileRepository = fileRepository;
        _mapper = mapper;
        _processorDataClient = processorDataClient;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFiles()
    {
        var files = await _fileRepository.GetFilesAsync();
        var mappedFiles = _mapper.Map<IEnumerable<FileDto>>(files);
        
        return Ok(mappedFiles);
    }

    [HttpGet("{id:int}", Name = "GetFileById")]
    public async Task<IActionResult> GetFileById(int id)
    {
        var file = await _fileRepository.GetFileAsync(id);
        
        if (file == null)
            return BadRequest();
        
        var mappedFile = _mapper.Map<FileDto>(file);

        return Ok(mappedFile);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFile(FileDto file)
    {
        var fileModel = _mapper.Map<FileModel>(file);

        await _fileRepository.CreateFileAsync(fileModel);
        await _fileRepository.SaveChangesAsync();
        
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

        return CreatedAtRoute(nameof(GetFileById), new { id = fileModel.Id }, fileModelToDto);
    }
}