using AutoMapper;
using FileProcessorService.Data;
using FileProcessorService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FileProcessorService.Controllers;

[Route("api/processor/[controller]")]
[ApiController]
public class ExtensionsController : ControllerBase
{
    private readonly IExtensionRepository _extensionRepository;
    private readonly IMapper _mapper;

    public ExtensionsController(IExtensionRepository extensionRepository, IMapper mapper)
    {
        _extensionRepository = extensionRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetExtensions()
    {
        var extensions = await _extensionRepository.GetAllAsync();

        var result = _mapper.Map<IEnumerable<ExtensionDto>>(extensions);
        
        return Ok(result);
    }
}