namespace FileProcessorService.FileConversion;

public class FileConverterFactory : IFileConverterFactory
{
    private readonly Dictionary<(string, string), IFileConverterStrategy> _strategies;

    public FileConverterFactory(IEnumerable<IFileConverterStrategy> strategies)
    {
        _strategies = strategies.ToDictionary(
            strategy => strategy switch
            {
                JpgToPng => ("image/jpeg", "image/png"),
                _ => throw new Exception("Unknown strategy")
            }
        );
    }
    
    public bool TryGetConverter(string sourceMediaType, string targetMediaType, out IFileConverterStrategy converter)
    {
        return _strategies.TryGetValue((sourceMediaType, targetMediaType), out converter);
    }
}