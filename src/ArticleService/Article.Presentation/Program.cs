using Article.Application;
using Article.Infrastructure;
using Article.Infrastructure.Database;
using Article.Presentation;
using Core.Api.Configurators;
using Core.Api.Middlewares;
using Core.Infrastructure.Extensions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = SerilogConfigurator.CreateLogger();
    builder.Host.UseSerilog((_, loggerConfiguration) => loggerConfiguration.ConfigureLogger());

    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddPresentationServices(builder.Configuration);
    builder.Services.AddApplicationServices();

    var app = builder.Build();

    app.UseStaticFiles();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using IServiceScope scope = app.Services.CreateScope();

        scope.ApplyMigration<SqlDbContext>();
    }

    app.UseCors();

    app.UseMiddleware<SerilogMiddleware>();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    //app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    Log.Error(ex, "Error while run application");
}
finally
{
    Log.CloseAndFlush();
}
