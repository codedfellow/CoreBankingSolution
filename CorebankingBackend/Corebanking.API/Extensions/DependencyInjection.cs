using Corebanking.API.Endpoints;
using Corebanking.API.OpenApi;
using Corebanking.Application.Features.Auth.Dtos;
using Microsoft.OpenApi;

namespace Corebanking.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddEndpointsApiExplorer();
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, ct) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = "CoreBanking API",
                        Version = "v1",
                        Description = "Core Banking System API"
                    };
                    return Task.CompletedTask;
                });

                // JWT security definition
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();

                //// ✅ Ensure schemas are included in components
                //options.AddSchemaTransformer((schema, context, ct) =>
                //{
                //    if (context.JsonTypeInfo.Type == typeof(RegisterRequest) ||
                //        context.JsonTypeInfo.Type == typeof(LoginRequest) ||
                //        context.JsonTypeInfo.Type == typeof(AuthResponse))
                //    {
                //        schema.Nullable = false;
                //    }
                //    return Task.CompletedTask;
                //});
            });

            return services;
        }
    }
}
