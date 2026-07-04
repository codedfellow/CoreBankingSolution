using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Features.Auth.Dtos
{
    public sealed record AuthResponse(
    Guid UserId,
    string Email,
    string AccessToken,
    DateTime AccessTokenExpiresAtUtc,
    string RefreshToken);
}
