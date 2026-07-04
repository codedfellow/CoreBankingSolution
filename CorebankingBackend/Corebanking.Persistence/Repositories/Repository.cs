using Corebanking.Application.Contracts.Data.Common;
using Corebanking.Domain.Common;
using Corebanking.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Corebanking.Persistence.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    {
        protected readonly BankingDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(BankingDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
            => await DbSet.FindAsync([id], ct);

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await DbSet.FirstOrDefaultAsync(predicate, ct);

        public async Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = DbSet;
            if (predicate is not null) query = query.Where(predicate);
            return await query.ToListAsync(ct);
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>>? predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
            int skip,
            int take,
            CancellationToken ct = default)
        {
            IQueryable<TEntity> query = DbSet;
            if (predicate is not null) query = query.Where(predicate);
            if (orderBy is not null) query = orderBy(query);

            return await query.Skip(skip).Take(take).ToListAsync(ct);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await DbSet.AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
            => predicate is null ? await DbSet.CountAsync(ct) : await DbSet.CountAsync(predicate, ct);

        public IQueryable<TEntity> AsNoTracking() => DbSet.AsNoTracking();

        public void Add(TEntity entity) => DbSet.Add(entity);
        public void AddRange(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);
        public void Update(TEntity entity) => DbSet.Update(entity);
        public void Remove(TEntity entity) => DbSet.Remove(entity);
        public void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);
    }
}
