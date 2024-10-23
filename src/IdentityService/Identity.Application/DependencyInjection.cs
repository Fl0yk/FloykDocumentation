using Identity.Application.Abstractions.Services;
using Identity.Application.Implementations.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
