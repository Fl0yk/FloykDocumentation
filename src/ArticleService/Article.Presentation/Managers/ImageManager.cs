using Article.Presentation.Shared.Options;
using Core.Managers;
using Microsoft.Extensions.Options;

namespace Article.Presentation.Managers;

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
        if (!string.IsNullOrEmpty(prevImageUrl))
        {
            RemoveImage(prevImageUrl);
        }

        string extension = Path.GetExtension(fileName);
        string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

        string filePath = Path.Combine(_options.WebRootPath, newFileName);

        using Stream fileStream = new FileStream(filePath, FileMode.Create);
        await source.CopyToAsync(fileStream, token);

        return $"{_options.Host}/{newFileName}";
    }

    private void RemoveImage(string imageUrl)
    {
        string? file = Directory.EnumerateFiles(Path.Combine(_options.WebRootPath))
                                        .FirstOrDefault(f => imageUrl.Contains(Path.GetFileName(f)));

        if (file != default)
        {
            File.Delete(file);
        }
    }
}
