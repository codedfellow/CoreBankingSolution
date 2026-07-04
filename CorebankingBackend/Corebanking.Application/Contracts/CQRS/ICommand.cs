using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Contracts.CQRS
{
    public interface ICommand<TResponse> { }

    public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
