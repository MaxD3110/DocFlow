using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace FileProcessorService.FileConversion;

public class CommonToJpg : IFileConverterStrategy
{
    public IEnumerable<(string, string)> SupportedConversions =>
    [
        ("image/bmp", "image/jpeg"),
        ("image/gif", "image/jpeg"),
        ("image/png", "image/jpeg")
    ];

    public async Task ConvertAsync(Stream input, Stream output)
    {
        using var image = await Image.LoadAsync(input);
        await image.SaveAsync(output, new JpegEncoder());
    }
}