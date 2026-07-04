using Corebanking.Application.Contracts.Data.Common;
using Corebanking.Persistence.Data;
using Corebanking.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly BankingDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        //private IAccountRepository? _accounts;
        //private IRepository<Transaction, Guid>? _transactions;

        public UnitOfWork(BankingDbContext context) => _context = context;

        //public IAccountRepository Accounts => _accounts ??= new AccountRepository(_context);
        //public IRepository<Transaction, Guid> Transactions => _transactions ??= new Repository<Transaction, Guid>(_context);

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);

        public async Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<CancellationToken, Task<TResult>> operation,
            CancellationToken ct = default)
        {
            // Reuse an already-open transaction if one exists (supports nested calls in the same use case)
            if (_currentTransaction is not null)
                return await operation(ct);

            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(ct);
                _currentTransaction = transaction;

                try
                {
                    var result = await operation(ct);
                    await _context.SaveChangesAsync(ct);
                    await transaction.CommitAsync(ct);
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
                finally
                {
                    _currentTransaction = null;
                }
            });
        }

        public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default)
        {
            await ExecuteInTransactionAsync<object?>(async token =>
            {
                await operation(token);
                return null;
            }, ct);
        }

        public async ValueTask DisposeAsync()
        {
            if (_currentTransaction is not null)
                await _currentTransaction.DisposeAsync();

            await _context.DisposeAsync();
        }
    }
}
