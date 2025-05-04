using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Core.Api.Configurators;

public static class SerilogConfigurator
{
    public static ILogger CreateLogger()
        => new Serilog.LoggerConfiguration().ConfigureLogger().CreateLogger();

    public static LoggerConfiguration ConfigureLogger(this LoggerConfiguration loggerConfiguration)
        => loggerConfiguration.Enrich.FromLogContext()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .WriteTo.Console();
    //.WriteTo.Elasticsearch(elasticsearchOptions).MinimumLevel
    //        .Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning);
}
