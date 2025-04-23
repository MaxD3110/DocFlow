using PdfSharp;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharp.Pdf;

namespace FileProcessorService.FileConversion;

public class HtmlToPdf : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("text/html", "application/pdf")];
    
    public async Task ConvertAsync(Stream input, Stream output)
    {
        using var reader = new StreamReader(input);
        var html = await reader.ReadToEndAsync();

        var document = new PdfDocument();
        PdfGenerator.AddPdfPages(document, html, PageSize.A4);
        document.Save(output);
    }
}