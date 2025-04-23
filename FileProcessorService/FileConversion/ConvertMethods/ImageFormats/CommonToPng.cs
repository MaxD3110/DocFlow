using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace FileProcessorService.FileConversion;

public class CommonToPng
{
    public IEnumerable<(string, string)> SupportedConversions =>
    [
        ("image/bmp", "image/png"),
        ("image/jpeg", "image/png")
    ];

    public async Task ConvertAsync(Stream input, Stream output)
    {
        using var image = await Image.LoadAsync(input);
        await image.SaveAsync(output, new PngEncoder());
    }
}