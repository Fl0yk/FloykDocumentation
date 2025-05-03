using Core.Infrastructure.Extensions;
using Core.Providers.Interfaces;
using Forum.Domain.Abstractions.Repositories;
using Forum.Infrastructure.BackgroundJobs.Question;
using Forum.Infrastructure.Data;
using Forum.Infrastructure.Repositories;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


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

        services.AddScoped<ISaveChangesInterceptor, SaveChangesInterceptor>();
        services.AddScoped<ITransactionProvider, TransactionProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<ApplicationDbContext>((sp, cfg) => {
            cfg.UseNpgsql(connectionString);
            cfg.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
        });

        services.ConfigureHangfire(connectionString);

        services.AddSignalR();

        RecurringJob.AddOrUpdate<CloseQuestionsBackgroundJob>($"Recuring-{nameof(CloseQuestionsBackgroundJob)}", x => x.CloseQuestionsAsync(25), Cron.Daily());

        return services;
    }
}
