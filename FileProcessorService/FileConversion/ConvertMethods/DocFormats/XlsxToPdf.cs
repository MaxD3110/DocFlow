using DocumentFormat.OpenXml.Packaging;
using QuestPDF.Fluent;

namespace FileProcessorService.FileConversion;

public class XlsxToPdf : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/pdf")];

    public Task ConvertAsync(Stream input, Stream output)
    {
        using var doc = SpreadsheetDocument.Open(input, false);
        var sheet = doc.WorkbookPart?.WorksheetParts.First().Worksheet;
        
        if (sheet == null)
            return Task.CompletedTask;
        
        var rows = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>();

        var data = rows.Select(r =>
            r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>()
                .Select(c => c.InnerText)
                .ToList()
        ).ToList();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Content().Table(table =>
                {
                    foreach (var row in data)
                    {
                        table.Cell().Row(rowIndex =>
                        {
                            foreach (var cell in row)
                                table.Cell().Element(c => c.Text(cell ?? ""));
                        });
                    }
                });
            });
        }).GeneratePdf(output);

        return Task.CompletedTask;
    }
}