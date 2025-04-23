namespace FileProcessorService.FileConversion;

public interface IFileConverterStrategy
{
    IEnumerable<(string InputMime, string OutputMime)> SupportedConversions { get; }

    public Task ConvertAsync(Stream input, Stream output);
}