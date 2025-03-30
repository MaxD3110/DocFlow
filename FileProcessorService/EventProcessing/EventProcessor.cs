using FileProcessorService.AsyncDataServices;
using FileProcessorService.DTOs;

namespace FileProcessorService.EventProcessing;

public class EventProcessor(IServiceScopeFactory scopeFactory) : IEventProcessor
{
    public async Task ProccessEventAsync(FileToConvertDto? file)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            var eventHelper = scope.ServiceProvider.GetRequiredService<IEventHelper>();

            if (file == null)
                return;

            if (!File.Exists(file.StoragePath))
                throw new FileNotFoundException("--> EventProcessor: Input file not found", file.StoragePath);

            try
            {
                var outputStream = await eventHelper.ConvertFile(file);

                var convertedFile = new FileConvertedDto();

                if (file.SaveAsNew && outputStream != null)
                    convertedFile = await eventHelper.TrySaveFile(file, outputStream);

                convertedFile.OriginalFileId = file.Id;
                convertedFile.TargetExtensionId = file.TargetExtensionId;

                await eventBus.SendConversionResult(convertedFile);

                //await eventHelper.AddToLog(file);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> EventProcessor: Failed to perform conversion! {e.Message}");
                throw;
            }
        }
    }
}