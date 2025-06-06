using AutoMapper;
using FileManagementService.DTOs;
using FileManagementService.Models;

namespace FileManagementService.Profiles;

public class FileProfile : Profile
{
    public FileProfile()
    {
        CreateMap<FileModel, FileDto>();
        CreateMap<FileDto, FileModel>();
        CreateMap<FileDto, FileToConvertDto>();
        CreateMap<FileModel, FileToConvertDto>();
        CreateMap<FileConvertedDto, FileModel>();
    }
}