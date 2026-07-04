using Corebanking.API.Endpoints;

namespace Corebanking.API.Extensions
{
    public static class MapEndpoints
    {
        public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapAuthEndpoints();

            return app;
        }
    }
}
