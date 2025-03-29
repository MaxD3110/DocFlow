namespace FileProcessorService.EventProcessing;

public interface IFileConverter
{
    public Task<byte[]> ConvertPdfToDoc(string filePath);
}