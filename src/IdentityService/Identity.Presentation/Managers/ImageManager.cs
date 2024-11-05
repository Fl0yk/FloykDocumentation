using Identity.Application.Abstractions.Managers;
using Identity.Presentation.Options.Models;
using Microsoft.Extensions.Options;

namespace Identity.Presentation.Managers;

public class ImageManager : IImageManager
{
    private readonly WWWRootOptions _options;

    public ImageManager(IOptions<WWWRootOptions> options)
    {
        _options = options.Value;
    }

    public async Task<string> SaveImageAsync(Stream source,
                                                string fileName,
                                                string? prevImageUrl,
                                                CancellationToken token = default)
    {
        if (prevImageUrl is not null)
        {
            RemoveImage(prevImageUrl);
        }

        string extension = Path.GetExtension(fileName);
        string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

        string filePath = Path.Combine(_options.WebRootPath, "Images", newFileName);

        using Stream fileStream = new FileStream(filePath, FileMode.Create);
        await source.CopyToAsync(fileStream, token);

        return $"{_options.Host}/Images/{newFileName}";
    }

    private void RemoveImage(string imageUrl)
    {
        string? file = Directory.EnumerateFiles(Path.Combine(_options.WebRootPath, "Images"))
                                        .FirstOrDefault(f => imageUrl.Contains(Path.GetFileName(f)));

        if (file != default)
        {
            File.Delete(file);
        }
    }
}
