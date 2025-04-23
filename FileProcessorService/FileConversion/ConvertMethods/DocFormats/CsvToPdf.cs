using QuestPDF.Fluent;

namespace FileProcessorService.FileConversion;

public class CsvToPdf : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("text/csv", "application/pdf")];

    public async Task ConvertAsync(Stream input, Stream output)
    {
        var reader = new StreamReader(input);
        var content = await reader.ReadToEndAsync();
        var lines = content.Split('\n');
        var rows = lines.Select(line => line.Split(',')).ToList();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Content().Table(table =>
                {
                    foreach (var row in rows)
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
    }
}