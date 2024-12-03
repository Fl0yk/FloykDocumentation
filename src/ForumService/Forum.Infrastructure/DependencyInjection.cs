using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Forum.Domain.Abstractions.Repositories;
using Forum.Infrastructure.Repositories;
using Forum.Domain.Abstractions.Services;
using Forum.Infrastructure.gRPC.Services.Clients;
using Hangfire;
using Forum.Infrastructure.BackgroundJobs.Question;


namespace Forum.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                                IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
                                                        ?? throw new ArgumentNullException("Connection string is not found");

        string hangfireConnection = configuration.GetConnectionString("HangfireConnection")
                                                        ?? throw new ArgumentNullException("Hangfire db connection string is not found");

        services.AddDbContext<ApplicationDbContext>(cfg => cfg.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserService, UserService>();

        services.AddHangfire(opt =>
        {
            opt.UseSqlServerStorage(hangfireConnection)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();
        });

        services.AddHangfireServer();

        RecurringJob.AddOrUpdate<CloseQuestionsBackgroundJob>(Guid.NewGuid().ToString(), x => x.CloseQuestionsAsync(1), Cron.Daily());

        return services;
    }
}
