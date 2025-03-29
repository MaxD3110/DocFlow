namespace FileProcessorService.EventProcessing;

public class FileConverter : IFileConverter
{
    public Task<byte[]> ConvertPdfToDoc(string filePath)
    {
        Console.WriteLine("Converting pdf to DOC");

        return Task.FromResult<byte[]>([]);
    }
}