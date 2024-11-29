using Identity.Application;
using Identity.DataAccess;
using Identity.DataAccess.Data;
using Identity.DataAccess.Data.Extensions;
using Identity.Infrastructure;
using Identity.Infrastructure.gRPC.Services.Servers;
using Identity.Presentation;
using Identity.Presentation.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseKestrel(conf => conf.ConfigureEndpointDefaults(endpoints => { endpoints.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2; }));

builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddPresentationServices(builder.Configuration);
builder.Services.AddInfrastructureServices();

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using IServiceScope scope = app.Services.CreateScope();

    scope.ApplyMigration<ApplicationDbContext>();
}

app.UseCors();

app.UseMiddleware<SerilogMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGrpcService<UserService>();

app.MapControllers();

app.Run();

Log.CloseAndFlush();