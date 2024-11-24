using Identity.Contracts.Abstractions.Services;
using Identity.Infrastructure.gRPC.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Infrastructure;

public static class DependenyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IArticleService, ArticleService>();
    }
}
