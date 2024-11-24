using Identity.Infrastructure.Shared.Options.Models;
using Microsoft.Extensions.Options;

namespace Identity.Presentation.Shared.Options.Setups;

public class UrlsOptionSetup : IConfigureOptions<UrlsOption>
{
    private readonly IConfiguration _configuration;

    public UrlsOptionSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(UrlsOption options)
    {
        var urls = _configuration.GetSection("Urls").Get<UrlsOption>()
                                            ?? throw new KeyNotFoundException("Can't read urls from appsettings.json");

        options.ForumUrl = urls.ForumUrl;
        options.ArticleUrl = urls.ArticleUrl;
        options.ApiGatewayUrl = urls.ApiGatewayUrl;
    }
}
