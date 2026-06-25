using Corebanking.Infrastructure.Configurations;
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
            Debug.WriteLine($"Connection String: {EnvironmentVariables.ConnectionString}");

            return services;
        }
    }
}
