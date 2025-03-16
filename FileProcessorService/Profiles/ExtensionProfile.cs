using AutoMapper;
using FileManagementService;
using FileProcessorService.DTOs;
using FileProcessorService.Models;

namespace FileProcessorService.Profiles;

public class ExtensionProfile : Profile
{
    public ExtensionProfile()
    {
        CreateMap<Extension, ExtensionDto>();
        CreateMap<ExtensionDto, Extension>();
        CreateMap<GrpcFileExtensionModel, Extension>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.ExtensionId));
    }
}