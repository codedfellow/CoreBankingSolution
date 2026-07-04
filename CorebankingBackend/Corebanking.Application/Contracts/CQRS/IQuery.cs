using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Contracts.CQRS
{
    public interface IQuery<TResponse> { }

    public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
