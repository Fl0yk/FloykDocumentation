using Article.Application;
using Article.Infrastructure;
using Article.Infrastructure.gRPC.Services.Servers;
using Article.Presentation;
using Article.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseKestrel(conf => conf.ConfigureEndpointDefaults(endpoints => { endpoints.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2; }));

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseMiddleware<SerilogMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<ArticleService>();

app.Run();
