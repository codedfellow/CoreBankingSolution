using Corebanking.Application.Common;
using Corebanking.Application.Contracts.Common;
using Corebanking.Application.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Features.Auth.Commands
{
    public sealed record RevokeAllRefreshTokensCommand(Guid UserId) : ICommand<ApiResult<bool>>;

    public sealed class RevokeAllRefreshTokensHandler(IRefreshTokenStore refreshTokenStore)
        : ICommandHandler<RevokeAllRefreshTokensCommand, ApiResult<bool>>
    {
        public async Task<ApiResult<bool>> Handle(RevokeAllRefreshTokensCommand command, CancellationToken ct)
        {
            await refreshTokenStore.RevokeAllAsync(command.UserId, ct);
            return ApiResult<bool>.Success(true);
        }
    }
}
