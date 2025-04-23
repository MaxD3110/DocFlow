using DocumentFormat.OpenXml.Packaging;
using QuestPDF.Fluent;

namespace FileProcessorService.FileConversion;

public class DocToPdf : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/pdf")];
    
    public Task ConvertAsync(Stream input, Stream output)
    {
        using var wordDoc = WordprocessingDocument.Open(input, false);
        var text = wordDoc.MainDocumentPart?.Document.InnerText;

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Content().Text(text).FontSize(12);
            });
        }).GeneratePdf(output);
        
        return Task.CompletedTask;
    }
}