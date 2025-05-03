namespace Core.Api.Models.Options;
public sealed class UrlsOption
{
    public string IdentityUrl { get; set; } = null!;

    public string ForumUrl { get; set; } = null!;

    public string ApiGatewayUrl { get; set; } = null!;

    public string ArticleUrl { get; set; } = null!;
}
