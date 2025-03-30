using AutoMapper;
using FileProcessorService.Data;
using FileProcessorService.DTOs;
using FileProcessorService.FileConversion;
using FileProcessorService.Models;
using FileProcessorService.SyncDataServices.Grpc;

namespace FileProcessorService.EventProcessing;

public class EventHelper : IEventHelper
{
    private readonly IExtensionRepository _extensionRepository;
    private readonly IFileLogRepository _fileLogRepository;
    private readonly IFileConverterFactory _converterFactory;
    private readonly IFileManagementDataClient _grpcClient;
    private readonly IMapper _mapper;
    private readonly string _storagePath;

    public EventHelper(IConfiguration config,
        IExtensionRepository extensionRepository,
        IFileLogRepository fileLogRepository,
        IFileConverterFactory converterFactory,
        IFileManagementDataClient grpcClient,
        IMapper mapper)
    {
        _extensionRepository = extensionRepository;
        _fileLogRepository = fileLogRepository;
        _converterFactory = converterFactory;
        _grpcClient = grpcClient;
        _mapper = mapper;
        _storagePath = config["StoredFilesPath"] ?? string.Empty;
    }

    public async Task<MemoryStream?> ConvertFile(FileToConvertDto file)
    { 
        var (sourceMediaType, targetMediaType) = await DetermineMediaType(file.SourceExtensionId, file.TargetExtensionId);
        
        if (!_converterFactory.TryGetConverter(sourceMediaType, targetMediaType, out var converter))
            throw new Exception("Unsupported conversion type");

        await using var inputStream = File.Open(file.StoragePath, FileMode.Open);
        var outputStream = new MemoryStream();

        await converter.ConvertAsync(inputStream, outputStream);

        return outputStream;
    }

    private async Task<(string, string)> DetermineMediaType(int sourceExtensionId, int targetExtensionId)
    {
        var currentExtension = await _extensionRepository.GetByExternalIdAsync(sourceExtensionId);
        var targetExtension = await _extensionRepository.GetByExternalIdAsync(targetExtensionId);

        if (currentExtension != null && targetExtension != null)
            return (currentExtension.MediaType, targetExtension.MediaType);

        var extensions = _grpcClient.GetExtensions();
            
        await _extensionRepository.SaveAllAsync(extensions);
            
        currentExtension = await _extensionRepository.GetByExternalIdAsync(sourceExtensionId);
        targetExtension = await _extensionRepository.GetByExternalIdAsync(targetExtensionId);

        return (currentExtension?.MediaType ?? string.Empty, targetExtension?.MediaType ?? string.Empty);
    }

    public async Task<FileConvertedDto> TrySaveFile(FileToConvertDto file, MemoryStream stream)
    {
        var extension = await _extensionRepository.GetByExternalIdAsync(file.TargetExtensionId);

        if (extension == null)
            return new FileConvertedDto();

        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
        var fileName = $"{fileNameWithoutExtension}_converted.{extension.FilenameExtension}";

        fileName = VerifyFileName(fileName);

        var filePath = Path.Combine(_storagePath, fileName);

        var outputFile = new FileConvertedDto
        {
            Name = fileName,
            StoragePath = filePath,
            FileSize = stream.Length
        };

        stream.Position = 0;
        await stream.FlushAsync();

        await using (var outputStream = File.Create(filePath))
        {
            await stream.CopyToAsync(outputStream);
            await outputStream.FlushAsync();
        }

        return outputFile;
    }

    private string VerifyFileName(string fileName)
    {
        var fileExtension = Path.GetExtension(fileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var newFileName = fileName;
        var attempt = 0;

        while (File.Exists(Path.Combine(_storagePath, newFileName)))
            newFileName = $"{fileNameWithoutExtension}({++attempt}){fileExtension}";

        return newFileName;
    }

    public async Task AddToLog(FileToConvertDto file)
    {
        var log = _mapper.Map<FileLogModel>(file);
        await _fileLogRepository.SaveFileLogAsync(log);
        await _fileLogRepository.SaveChangesAsync();
    }
}