namespace FileProcessorService.FileConversion;

public interface IFileConverterStrategy
{
    public Task ConvertAsync(Stream input, Stream output);
}