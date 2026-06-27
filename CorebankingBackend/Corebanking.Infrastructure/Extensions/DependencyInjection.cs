using Corebanking.Application.Contracts.Configurations;
using Corebanking.Infrastructure.Configurations;
using Corebanking.Persistence.Data;
using Corebanking.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Corebanking.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //Debug.WriteLine($"Connection String: {EnvironmentVariables.ConnectionString}");

            services.AddSingleton<IInfrastructureConfigrations, InfrastructureConfigrations>();

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(
                    options =>
                    {
                        ConfigureIdentityOptions(options);
                    })
                .AddEntityFrameworkStores<BankingDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        private static void ConfigureIdentityOptions(
       IdentityOptions options)
        {
            options.Password.RequiredLength = 6;

            options.Password.RequireDigit = true;

            options.Password.RequireUppercase = true;

            options.Password.RequireLowercase = true;

            options.Password.RequireNonAlphanumeric = false;


            options.Lockout.MaxFailedAccessAttempts = 5;

            options.Lockout.DefaultLockoutTimeSpan =
                TimeSpan.FromMinutes(15);
        }
    }
}
