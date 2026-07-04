using Corebanking.API.Endpoints;
using Corebanking.Application.Features.Auth.Dtos;
using Microsoft.OpenApi;

namespace Corebanking.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CoreBanking API",
                    Version = "v1",
                    Description = "Core Banking System API"
                });

                // Security definition — tells Swagger UI to show the Authorize button
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token here. Do not add 'Bearer' prefix."
                });

                // Security requirement — tells Swagger UI to send the token on every request
                options.AddSecurityRequirement(document =>
                {
                    var requirement = new OpenApiSecurityRequirement();
                    requirement.Add(
                        new OpenApiSecuritySchemeReference("Bearer"),
                        new List<string>());
                    return requirement;
                });
            });

            return services;
        }
    }
}
