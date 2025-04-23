using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FileProcessorService.FileConversion;

public class CsvToXlsx : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("text/csv", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")];
    
    public async Task ConvertAsync(Stream input, Stream output)
    {
        var reader = new StreamReader(input);
        var content = await reader.ReadToEndAsync();
        var lines = content.Split('\n');

        using var spreadsheet = SpreadsheetDocument.Create(input, SpreadsheetDocumentType.Workbook);
        var workbookPart = spreadsheet.AddWorkbookPart();
        
        workbookPart.Workbook = new Workbook();

        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        var sheetData = new SheetData();
        worksheetPart.Worksheet = new Worksheet(sheetData);

        if (spreadsheet.WorkbookPart == null)
            return; 

        var sheets = spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());
        sheets.Append(new Sheet { Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" });

        foreach (var line in lines)
        {
            var row = new Row();
            foreach (var cellValue in line.Split(","))
            {
                row.Append(new Cell
                {
                    DataType = CellValues.String,
                    CellValue = new CellValue(cellValue)
                });
            }
            sheetData.Append(row);
        }

        workbookPart.Workbook.Save(output);
    }
}