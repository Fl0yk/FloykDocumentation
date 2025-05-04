using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigureHangfire(this IServiceCollection services, string connectionString)
    {
        services.AddHangfire(x => x.UsePostgreSqlStorage(connectionString,
                new PostgreSqlStorageOptions
                {
                    InvisibilityTimeout = TimeSpan.FromMinutes(60)
                }));

        services.AddHangfireServer();

        return services;
    }
}
