using Identity.Application;
using Identity.DataAccess;
using Identity.Infrastructure;
using Identity.Infrastructure.gRPC.Services;
using Identity.Presentation;
using Identity.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

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
}

app.UseCors();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGrpcService<UserService>();

app.MapControllers();

app.Run();
