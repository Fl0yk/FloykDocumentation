using Core.Api.Configurators;
using Core.Api.Middlewares;
using Core.Infrastructure.Extensions;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.Database;
using Identity.Presentation;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = SerilogConfigurator.CreateLogger();
    builder.Host.UseSerilog((_, loggerConfiguration) => loggerConfiguration.ConfigureLogger());

    builder.Services.AddApplicationServices();
    builder.Services.AddPresentationServices(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);

    var app = builder.Build();

    Log.Information("Starting web host...");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using IServiceScope scope = app.Services.CreateScope();

        scope.ApplyMigration<ApplicationDbContext>();
    }

    app.UseStaticFiles();

    app.UseCors();

    app.UseMiddleware<SerilogMiddleware>();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    //app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly!..");

    throw;
}
finally
{
    Log.CloseAndFlush();
}