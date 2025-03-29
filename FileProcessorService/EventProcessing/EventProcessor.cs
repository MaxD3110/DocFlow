using System.Text.Json;
using AutoMapper;
using FileManagementService;
using FileProcessorService.AsyncDataServices;
using FileProcessorService.Data;
using FileProcessorService.DTOs;
using FileProcessorService.Models;

namespace FileProcessorService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    
    public async Task ProccessEventAsync(FileToConvertDto? file)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var fileLogRepository = scope.ServiceProvider.GetRequiredService<IFileLogRepository>();
            var fileConverter = scope.ServiceProvider.GetRequiredService<IFileConverter>();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            if (file == null)
                return;

            try
            {
                byte[] fileBytes = [];
            
                switch (file.Event)
                {
                    case ConversionType.PdfToDoc:
                        fileBytes = await fileConverter.ConvertPdfToDoc(file.StoragePath);
                        break;
                    case ConversionType.Unknown:
                    case ConversionType.DocToPdf:
                    case ConversionType.ImageToPng:
                    case ConversionType.PngToJpeg:
                    case ConversionType.Mp3ToWav:
                    default:
                        Console.WriteLine("--> EventProcessor: Undetermined media type");
                        return;
                }

                var convertedFile = new FileConvertedDto();

                await eventBus.SendConversionResult(convertedFile);
                
                var log = _mapper.Map<FileLogModel>(file);
                await fileLogRepository.SaveFileLogAsync(log);
                await fileLogRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> EventProcessor: Failed to save file to log {e.Message}");
                throw;
            }
        }
    }
}