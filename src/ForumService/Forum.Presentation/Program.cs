using Forum.Application;
using Forum.Infrastructure;
using Forum.Infrastructure.Extensions;
using Forum.Infrastructure.SignalR.Hubs;
using Forum.Presentation;
using Forum.Presentation.Middlewares;
using Forum.Presentation.Shared.Filters;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentetionServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangFireAuthorizationFilter() }
    });

    using IServiceScope scope = app.Services.CreateScope();

    scope.ApplyMigration<ApplicationDbContext>();
}

app.UseCors();

app.UseMiddleware<SerilogMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<QuestionsHub>("/questions-hub");

app.MapControllers();

app.Run();