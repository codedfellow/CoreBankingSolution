//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.OpenApi;
//using Microsoft.OpenApi;

//namespace Corebanking.API.OpenApi
//{
//    public sealed class BearerSecuritySchemeTransformer(
//    IAuthenticationSchemeProvider authenticationSchemeProvider)
//    : IOpenApiDocumentTransformer
//    {
//        public async Task TransformAsync(
//            OpenApiDocument document,
//            OpenApiDocumentTransformerContext context,
//            CancellationToken ct)
//        {
//            var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

//            if (!authenticationSchemes.Any(s => s.Name == JwtBearerDefaults.AuthenticationScheme))
//                return;

//            // ✅ Never replace document.Components — it already holds all registered schemas
//            // Only initialize it if it truly doesn't exist yet
//            document.Components ??= new OpenApiComponents();
//            // ✅ Only initialize SecuritySchemes if null, don't touch anything else
//            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

//            document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
//            {
//                Type = SecuritySchemeType.Http,
//                Scheme = "bearer",
//                BearerFormat = "JWT",
//                In = ParameterLocation.Header,
//                Name = "Authorization",
//                Description = "Enter your JWT access token here"
//            };

//            var securityRequirement = new OpenApiSecurityRequirement();
//            securityRequirement.Add(
//                new OpenApiSecuritySchemeReference("Bearer"),
//                new List<string>());

//            foreach (var path in document.Paths.Values)
//            {
//                foreach (var operation in path.Operations.Values)
//                {
//                    operation.Security ??= [];
//                    operation.Security.Add(securityRequirement);
//                }
//            }
//        }
//    }
//}
