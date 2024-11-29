using Forum.Application;
using Forum.Presentation;
using Forum.Infrastructure;
using Forum.Presentation.Middlewares;
using Forum.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseKestrel(conf => conf.ConfigureEndpointDefaults(endpoints => { endpoints.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2; }));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentetionServices(builder.Configuration);

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.Run();