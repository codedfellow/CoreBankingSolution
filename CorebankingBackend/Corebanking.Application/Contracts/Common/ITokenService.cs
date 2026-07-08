using Corebanking.Application.Common;
using Corebanking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Contracts.Common
{
    public sealed record TokenPrincipal(Guid UserId, string Email);

    public interface ITokenService
    {
        (string token, DateTime expiresAtUtc) GenerateAccessToken(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        IList<string> roles);
        string GenerateRefreshToken();
        ApiResult<TokenPrincipal> GetPrincipalFromExpiredToken(string accessToken);
    }
}
