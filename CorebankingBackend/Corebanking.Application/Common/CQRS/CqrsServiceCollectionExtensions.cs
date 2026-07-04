using Corebanking.Application.Contracts.CQRS;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Corebanking.Application.Common.CQRS
{
    public static class CqrsServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomCqrs(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddScoped<IDispatcher, Dispatcher>();

            var openHandlerInterfaces = new[] { typeof(ICommandHandler<,>), typeof(IQueryHandler<,>) };

            foreach (var assembly in assemblies)
            {
                var handlerTypes = assembly.GetTypes()
                    .Where(t => t is { IsAbstract: false, IsInterface: false })
                    .SelectMany(t => t.GetInterfaces()
                        .Where(i => i.IsGenericType && openHandlerInterfaces.Contains(i.GetGenericTypeDefinition()))
                        .Select(i => new { Interface = i, Implementation = t }));

                foreach (var h in handlerTypes)
                    services.AddScoped(h.Interface, h.Implementation);
            }

            return services;
        }
    }
}
