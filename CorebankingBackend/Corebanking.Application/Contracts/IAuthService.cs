using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Contracts
{
    public interface IAuthService
    {
        Task<AuthServiceResult> RegisterAsync(
            string firstName,
            string lastName,
            string email,
            string password,
            CancellationToken ct = default);

        Task<AuthServiceResult> LoginAsync(
            string email,
            string password,
            CancellationToken ct = default);

        Task<AuthServiceResult> GetUserByIdAsync(Guid userId, CancellationToken ct = default);
    }

    public sealed record AuthServiceResult(
        bool Succeeded,
        Guid? UserId,
        string? Email,
        string? FirstName,
        string? LastName,
        IList<string>? Roles,
        string[]? Errors,
        bool IsLockedOut = false,
        bool IsActive = true);
}
