using AutoMapper;
using FileManagementService.Data;
using FileManagementService.DTOs;
using FileManagementService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileManagementService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExtensionsController : ControllerBase
{
    private readonly IRepository<Extension> _extensionRepository;
    private readonly IMapper _mapper;

    public ExtensionsController(IRepository<Extension> extensionRepository, IMapper mapper)
    {
        _extensionRepository = extensionRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetExtensions()
    {
        var extensions = await _extensionRepository.Query()
            .ToListAsync();

        var mappedFiles = _mapper.Map<IEnumerable<ExtensionDto>>(extensions);
        
        return Ok(mappedFiles);
    }
}