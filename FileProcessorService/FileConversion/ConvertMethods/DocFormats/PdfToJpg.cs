using System.Drawing.Imaging;
using PdfiumViewer;

namespace FileProcessorService.FileConversion;

public class PdfToJpg : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("application/pdf", "image/jpeg")];
    
    public Task ConvertAsync(Stream input, Stream output)
    {
        using var document = PdfDocument.Load(input);
        using var image = document.Render(0, 300, 300, true);
        image.Save(output, ImageFormat.Jpeg);
        
        return Task.CompletedTask;
    }
}