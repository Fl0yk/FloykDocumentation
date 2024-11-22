using Article.Infrastructure.Options.Models;
using Microsoft.Extensions.Options;

namespace Article.Presentation.Shared.Options.Setups;

public class UrlsOptionSetup : IConfigureOptions<UrlsOption>
{
    private readonly IConfiguration _configuration;

    public UrlsOptionSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(UrlsOption options)
    {
        var section = _configuration.GetSection("Urls")
                            ?? throw new KeyNotFoundException("Can't read urls from appsettings.json");

        options.IdentityUrl = section["IdentityService"]!;
    }
}
