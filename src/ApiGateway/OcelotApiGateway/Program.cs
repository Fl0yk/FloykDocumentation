using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGateway;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseKestrel(conf => conf.ConfigureEndpointDefaults(endpoints => {  }));

var env = builder.Environment;


//builder.Configuration.AddOcelotWithSwaggerSupport(options =>
//{
//    options.Folder = "Configuration";
//});

builder.Configuration.AddJsonFile("ocelot.json", optional: false);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
}

app.UseCors();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseOcelot().Wait();

app.Run();
