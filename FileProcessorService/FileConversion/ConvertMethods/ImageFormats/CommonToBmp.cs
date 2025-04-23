using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;

namespace FileProcessorService.FileConversion;

public class CommonToBmp
{
    public IEnumerable<(string, string)> SupportedConversions =>
    [
        ("image/jpeg", "image/bmp")
    ];

    public async Task ConvertAsync(Stream input, Stream output)
    {
        using var image = await Image.LoadAsync(input);
        await image.SaveAsync(output, new BmpEncoder());
    }
}