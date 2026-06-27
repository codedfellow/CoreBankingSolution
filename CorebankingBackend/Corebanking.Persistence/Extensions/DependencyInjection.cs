using Corebanking.Application.Contracts.Configurations;
using Corebanking.Persistence.Configurations;
using Corebanking.Persistence.Data;
using Corebanking.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Corebanking.Persistence.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddSingleton<IPersistenceConfigurations, PersistenceConfigurations>();

            services.AddDbContext<BankingDbContext>(
            options =>
            {
                options.UseNpgsql(
                    EnvironmentVariables.ConnectionString,
                    npgsql =>
                    {
                        npgsql.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay:
                                TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null);
                    });
            });

            return services;
        }
    }
}
