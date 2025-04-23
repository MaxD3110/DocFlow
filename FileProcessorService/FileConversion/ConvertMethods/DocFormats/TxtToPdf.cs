using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace FileProcessorService.FileConversion;

public class TxtToPdf : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("text/plain", "application/pdf")];
    
    public async Task ConvertAsync(Stream input, Stream output)
    {
        var reader = new StreamReader(input);
        var text = await reader.ReadToEndAsync();
        var lines = text.Split('\n');
        var pdf = new PdfDocument();
        var page = pdf.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Courier New", 10);
        double y = 40;

        foreach (var line in lines)
        {
            gfx.DrawString(line, font, XBrushes.Black, new XRect(40, y, page.Width - 80, 20), XStringFormats.TopLeft);
            y += 15;
        }

        pdf.Save(output);
    }
}