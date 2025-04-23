using System.Drawing.Imaging;
using PdfiumViewer;

namespace FileProcessorService.FileConversion;

public class PdfToPng : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("application/pdf", "image/png")];
    
    public Task ConvertAsync(Stream input, Stream output)
    {
        using var document = PdfDocument.Load(input);
        using var image = document.Render(0, 300, 300, true);
        image.Save(output, ImageFormat.Png);
        
        return Task.CompletedTask;
    }
}