using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace FileProcessorService.FileConversion;

public class JpgToPng : IFileConverterStrategy
{
    public async Task ConvertAsync(Stream input, Stream output)
    {
        using var image = await Image.LoadAsync(input);
        await image.SaveAsync(output, new PngEncoder());
    }
}