using Forum.Application;
using Forum.Presentation;
using Forum.Infrastructure;
using Forum.Presentation.Middlewares;
using Forum.Infrastructure.Extensions;

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

    using IServiceScope scope = app.Services.CreateScope();

    scope.ApplyMigration<ApplicationDbContext>();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseMiddleware<SerilogMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
