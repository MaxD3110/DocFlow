using AutoMapper;
using FileManagementService.Data;
using FileManagementService.DTOs;
using FileManagementService.Models;

namespace FileManagementService.EventProcessing;

public class EventProcessor(IServiceScopeFactory scopeFactory) : IEventProcessor
{

    public async Task ProccessEventAsync(FileConvertedDto? file)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var extensionRepository = scope.ServiceProvider.GetRequiredService<IRepository<Extension>>();
            var fileRepository = scope.ServiceProvider.GetRequiredService<IRepository<FileModel>>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            
            if (file == null)
                return;
            
            var extension = await extensionRepository.GetByIdAsync(file.TargetExtensionId);

            if (extension == null)
                return;
            
            var fileToSave = mapper.Map<FileModel>(file);
            
            fileToSave.UploadedAt = DateTime.UtcNow;
            fileToSave.Extension = extension;

            await fileRepository.CreateAsync(fileToSave);
        }
    }
}