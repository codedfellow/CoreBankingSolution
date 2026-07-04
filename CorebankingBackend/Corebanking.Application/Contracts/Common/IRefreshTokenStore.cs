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
    }
}
