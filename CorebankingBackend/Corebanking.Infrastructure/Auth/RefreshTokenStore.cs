using Corebanking.Application.Contracts.Common;
using Corebanking.Domain.Entities;
using Corebanking.Infrastructure.Configurations;
using Corebanking.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Infrastructure.Auth
{
    public sealed class RefreshTokenStore(BankingDbContext db) : IRefreshTokenStore
    {

        public async Task SaveAsync(Guid userId, string token, CancellationToken ct)
        {
            db.RefreshTokens.Add(new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(EnvironmentVariables.RefreshTokenDays)
            });
            await db.SaveChangesAsync(ct);
        }

        public async Task<Guid?> ValidateAndConsumeAsync(string token, CancellationToken ct)
        {
            var entity = await db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, ct);
            if (entity is null || !entity.IsActive) return null;

            entity.RevokedAtUtc = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);
            return entity.UserId;
        }

        public async Task RevokeAsync(string token, CancellationToken ct)
        {
            var entity = await db.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token, ct);

            if (entity is null || !entity.IsActive)
                return;

            entity.RevokedAtUtc = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);
        }

        public async Task<Guid?> GetOwnerAsync(string token, CancellationToken ct = default)
        {
            var entity = await db.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == token, ct);

            return entity?.UserId;
        }

        public async Task RevokeAllAsync(Guid userId, CancellationToken ct = default)
        {
            var activeTokens = await db.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAtUtc == null && x.ExpiresAtUtc > DateTime.UtcNow)
            .ToListAsync(ct);

            if (!activeTokens.Any())
                return;

            var now = DateTime.UtcNow;
            foreach (var token in activeTokens)
                token.RevokedAtUtc = now;

            await db.SaveChangesAsync(ct);
        }
    }
}
