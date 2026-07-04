using Corebanking.API.Extensions;
using Corebanking.Application.Extensions;
using Corebanking.Infrastructure.Extensions;
using Corebanking.Persistence.Data;
using Corebanking.Persistence.Extensions;
using Corebanking.Persistence.Identity;
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
    app.MapOpenApi();

    // Swagger UI pointed at the built-in OpenAPI endpoint
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "CoreBanking API v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "CoreBanking API";
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.EnableFilter();
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapAllEndpoints();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
    await db.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();
    foreach (var role in new[] { "Customer", "BackOffice" })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new AppIdentityRole(role));
    }
}

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
