using Corebanking.API.Extensions;
using Corebanking.Application.Extensions;
using Corebanking.Infrastructure.Extensions;
using Corebanking.Persistence.Data;
using Corebanking.Persistence.Extensions;
using Corebanking.Persistence.Identity;
using Corebanking.Shared.Constants;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
object value = builder.Services.AddApi(builder.Configuration).AddApplication().AddInfrastructure().AddPersistence();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Built-in OpenAPI JSON endpoint
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // ✅ Swashbuckle serves JSON here, not /openapi/v1.json
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreBanking API v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "CoreBanking API";
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.EnableFilter();
        options.EnablePersistAuthorization();
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapAllEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
    await db.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();
    foreach (var role in new[] { UserRolesConsts.Customer, UserRolesConsts.BackOffice })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new AppIdentityRole(role));
    }
}

app.Run();
