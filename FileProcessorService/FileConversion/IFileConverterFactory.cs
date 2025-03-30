namespace FileProcessorService.FileConversion;

public interface IFileConverterFactory
{
    public bool TryGetConverter(string sourceMediaType, string targetMediaType, out IFileConverterStrategy converter);
}