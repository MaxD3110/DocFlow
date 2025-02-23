using AutoMapper;
using FileProcessorService.DTOs;
using FileProcessorService.Models;

namespace FileProcessorService.Profiles;

public class FileProfile : Profile
{
    public FileProfile()
    {
        CreateMap<FileLogModel, FileLogDto>();
        CreateMap<FileLogDto, FileLogModel>();
        CreateMap<FileLogModel, FileToConvertDto>();
        CreateMap<FileToConvertDto, FileLogModel>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
    }
}