using AutoMapper;
using FileManagementService.DTOs;
using FileManagementService.Models;

namespace FileManagementService.Profiles;

public class ExtensionProfile : Profile
{
    public ExtensionProfile()
    {
        CreateMap<Extension, ExtensionDto>();
        CreateMap<ExtensionDto, Extension>();
        CreateMap<Extension, GrpcFileExtensionModel>()
            .ForMember(dest => dest.ExtensionId, opt => opt.MapFrom(src => src.Id));
    }
}