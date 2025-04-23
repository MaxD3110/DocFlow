using Xceed.Words.NET;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace FileProcessorService.FileConversion;

public class PdfToDoc : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")];
    
    public Task ConvertAsync(Stream input, Stream output)
    {
        var doc = DocX.Load(input);
        var pdf = new PdfDocument();
        var page = pdf.AddPage();
        var gfx = XGraphics.FromPdfPage(page);

        var text = doc.Text;
        gfx.DrawString(text, new XFont("Arial", 12), XBrushes.Black,
            new XRect(40, 40, page.Width - 80, page.Height - 80),
            XStringFormats.TopLeft);

        pdf.Save(output);
        
        return Task.CompletedTask;
    }
}