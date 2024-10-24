﻿using Identity.Application.Abstractions.Services;
using Identity.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
