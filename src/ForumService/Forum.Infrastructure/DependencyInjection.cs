using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Forum.Domain.Abstractions.Repositories;
using Forum.Infrastructure.Repositories;
using Forum.Domain.Abstractions.Services;
using Forum.Infrastructure.gRPC.Services.Clients;
using Hangfire;
using Forum.Infrastructure.BackgroundJobs.Question;
using Hangfire.SqlServer;
using Microsoft.Data.SqlClient;


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
            opt.UseSqlServerStorage(connectionString)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();
        });

        JobStorage.Current = new SqlServerStorage(connectionString);

        services.AddHangfireServer();

        services.AddSignalR();
        //Generate("ForumTasks", hangfireConnection);

        //RecurringJob.AddOrUpdate<CloseQuestionsBackgroundJob>("4699754f-79de-4b23-8ba5-dac5cf0357da", x => x.CloseQuestionsAsync(25), Cron.Daily());

        return services;
    }
}
