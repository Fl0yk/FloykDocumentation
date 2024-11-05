namespace Identity.Application.Abstractions.Managers;

public interface IImageManager
{
    /// <param name="source">Stream with image data</param>
    /// <param name="fileName">Original file name with extention</param>
    /// <param name="prevImageUrl">Link to previous avatar</param>
    /// <returns>Url to new avatar image</returns>
    public Task<string> SaveImageAsync(Stream source, string fileName, string? prevImageUrl, CancellationToken token = default);
}
