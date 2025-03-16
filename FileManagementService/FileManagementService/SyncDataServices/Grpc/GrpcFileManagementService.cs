using AutoMapper;
using FileManagementService.Data;
using FileManagementService.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace FileManagementService.SyncDataServices.Grpc;

public class GrpcFileManagementService : GrpcFileExtension.GrpcFileExtensionBase
{
    private readonly IRepository<Extension> _extensionRepository;
    private readonly IMapper _mapper;

    public GrpcFileManagementService(IRepository<Extension> extensionRepository, IMapper mapper)
    {
        _extensionRepository = extensionRepository;
        _mapper = mapper;
    }

    public override async Task<FileExtensionResponse> GetAllFileExtensions(GetAllFileExtensionsRequest request, ServerCallContext context)
    {
        var response = new FileExtensionResponse();
        var extensions = await _extensionRepository.Query()
            .ToListAsync();

        foreach (var extension in extensions)
        {
            response.Extension.Add(_mapper.Map<GrpcFileExtensionModel>(extension));
        }

        return response;
    }
}