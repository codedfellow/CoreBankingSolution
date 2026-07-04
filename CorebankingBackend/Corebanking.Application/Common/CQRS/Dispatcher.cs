using Corebanking.Application.Contracts.CQRS;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Common.CQRS
{
    public sealed class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _provider;

        // Cache reflection lookups so we don't pay the cost on every request
        private static readonly ConcurrentDictionary<Type, Type> CommandHandlerTypeCache = new();
        private static readonly ConcurrentDictionary<Type, Type> QueryHandlerTypeCache = new();

        public Dispatcher(IServiceProvider provider) => _provider = provider;

        public Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            var commandType = command.GetType();
            var handlerType = CommandHandlerTypeCache.GetOrAdd(commandType,
                t => typeof(ICommandHandler<,>).MakeGenericType(t, typeof(TResponse)));

            var handler = _provider.GetService(handlerType)
                ?? throw new InvalidOperationException($"No command handler registered for {commandType.Name}");

            return InvokeHandle<TResponse>(handler, handlerType, command, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            var queryType = query.GetType();
            var handlerType = QueryHandlerTypeCache.GetOrAdd(queryType,
                t => typeof(IQueryHandler<,>).MakeGenericType(t, typeof(TResponse)));

            var handler = _provider.GetService(handlerType)
                ?? throw new InvalidOperationException($"No query handler registered for {queryType.Name}");

            return InvokeHandle<TResponse>(handler, handlerType, query, cancellationToken);
        }

        private static Task<TResponse> InvokeHandle<TResponse>(object handler, Type handlerType, object request, CancellationToken ct)
        {
            var method = handlerType.GetMethod("Handle")!;
            return (Task<TResponse>)method.Invoke(handler, [request, ct])!;
        }
    }
}
