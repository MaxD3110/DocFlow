using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace FileProcessorService.FileConversion;

public class DocToHtml : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions => [("application/vnd.openxmlformats-officedocument.wordprocessingml.document", "text/html")];

    public async Task ConvertAsync(Stream input, Stream output)
    {
        using var wordDoc = WordprocessingDocument.Open(input, false);
        var body = wordDoc.MainDocumentPart?.Document.Body;
        
        if (body == null)
            return;

        var sb = new StringBuilder();
        sb.Append("<html><body>");

        foreach (var para in body.Elements<Paragraph>())
        {
            sb.Append("<p>");
            foreach (var run in para.Elements<Run>())
            {
                var text = run.GetFirstChild<Text>()?.Text;
                if (text != null)
                {
                    // Optional: add bold, italic, etc.
                    if (run.RunProperties?.Bold != null)
                        sb.Append("<b>");
                    if (run.RunProperties?.Italic != null)
                        sb.Append("<i>");

                    sb.Append(System.Net.WebUtility.HtmlEncode(text));

                    if (run.RunProperties?.Italic != null)
                        sb.Append("</i>");
                    if (run.RunProperties?.Bold != null)
                        sb.Append("</b>");
                }
            }
            sb.Append("</p>");
        }

        sb.Append("</body></html>");
        
        var writer = new StreamWriter(output, Encoding.UTF8);
        await writer.WriteAsync(sb.ToString());
        await writer.FlushAsync();

        output.Position = 0;
    }
}