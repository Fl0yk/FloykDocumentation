using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace Forum.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentetionServices(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddFluentValidationAutoValidation();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
