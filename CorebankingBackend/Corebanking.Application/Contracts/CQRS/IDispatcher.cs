using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Contracts.CQRS
{
    public interface IDispatcher
    {
        Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
        Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    }
}
