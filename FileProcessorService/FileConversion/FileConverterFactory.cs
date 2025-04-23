namespace FileProcessorService.FileConversion;

public class FileConverterFactory : IFileConverterFactory
{
    private readonly Dictionary<(string, string), IFileConverterStrategy> _strategies;

    public FileConverterFactory(IEnumerable<IFileConverterStrategy> strategies)
    {
        _strategies = new Dictionary<(string, string), IFileConverterStrategy>();

        foreach (var strategy in strategies)
        {
            foreach (var (input, output) in strategy.SupportedConversions)
            {
                _strategies[(input, output)] = strategy;
            }
        }
    }
    
    public bool TryGetConverter(string sourceMediaType, string targetMediaType, out IFileConverterStrategy converter)
    {
        return _strategies.TryGetValue((sourceMediaType, targetMediaType), out converter);
    }
}