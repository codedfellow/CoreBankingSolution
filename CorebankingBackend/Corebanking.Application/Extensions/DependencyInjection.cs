using Corebanking.Application.Common.CQRS;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Corebanking.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddCustomCqrs(assembly);
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
