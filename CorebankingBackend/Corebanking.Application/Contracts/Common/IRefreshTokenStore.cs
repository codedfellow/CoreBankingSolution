using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Contracts.Common
{
    public interface IRefreshTokenStore
    {
        Task SaveAsync(Guid userId, string token, CancellationToken ct);
        Task<Guid?> ValidateAndConsumeAsync(string token, CancellationToken ct); // returns userId if valid
        Task RevokeAsync(string token, CancellationToken ct);
        /// <summary>Returns the userId that owns this token, regardless of its active state.</summary>
        Task<Guid?> GetOwnerAsync(string token, CancellationToken ct = default);
        Task RevokeAllAsync(Guid userId, CancellationToken ct = default);
    }
}
