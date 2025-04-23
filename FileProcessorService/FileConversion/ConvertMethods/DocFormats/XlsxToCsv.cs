using ClosedXML.Excel;

namespace FileProcessorService.FileConversion;

public class XlsxToCsv : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv")];
    
    public async Task ConvertAsync(Stream input, Stream output)
    {
        using var workbook = new XLWorkbook(input);
        var worksheet = workbook.Worksheet(1);
        await using var writer = new StreamWriter(output);

        foreach (var row in worksheet.RowsUsed())
        {
            var cells = row.Cells().Select(c => c.Value.ToString().Replace(",", " "));
            await writer.WriteLineAsync(string.Join(",", cells));
        }
    }
}