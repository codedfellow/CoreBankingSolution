using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Contracts.Data.Common
{
    public interface IUnitOfWork
    {

        /// <summary>Persists tracked changes. Returns number of affected rows.</summary>
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        /// <summary>
        /// Runs the given work inside a DB transaction. Commits on success, rolls back on exception.
        /// Use this whenever a use case spans multiple aggregate saves that must be atomic
        /// (e.g. debit one account + credit another + write a transaction record).
        /// </summary>
        Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<CancellationToken, Task<TResult>> operation,
            CancellationToken ct = default);

        Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> operation,
            CancellationToken ct = default);
    }
}
