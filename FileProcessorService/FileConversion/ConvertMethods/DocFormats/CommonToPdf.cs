using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FileProcessorService.FileConversion;

public class CommonToPdf
{
    public IEnumerable<(string, string)> SupportedConversions =>
    [
        ("image/jpeg", "application/pdf"),
        ("image/png", "application/pdf")
    ];
    
    public async Task ConvertAsync(Stream input, Stream output)
    {
        using var image = await Image.LoadAsync<Rgba32>(input);
        using var ms = new MemoryStream();
        await image.SaveAsJpegAsync(ms);
        ms.Position = 0;

        using var document = new PdfDocument();
        var page = document.AddPage();

        page.Width = XUnit.FromPoint(image.Width);
        page.Height = XUnit.FromPoint(image.Height);

        var gfx = XGraphics.FromPdfPage(page);
        using var xImage = XImage.FromStream(() => ms);
        gfx.DrawImage(xImage, 0, 0, page.Width, page.Height);

        document.Save(output);
    }
}