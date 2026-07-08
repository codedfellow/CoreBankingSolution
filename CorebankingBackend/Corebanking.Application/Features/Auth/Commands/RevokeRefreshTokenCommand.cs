using Corebanking.Application.Common;
using Corebanking.Application.Contracts.Common;
using Corebanking.Application.Contracts.CQRS;
using FluentValidation;

namespace Corebanking.Application.Features.Auth.Commands
{
    public sealed record RevokeRefreshTokenCommand(
    string RefreshToken,
    Guid RequestingUserId) : ICommand<ApiResult<bool>>;

    public sealed class RevokeRefreshTokenValidator : AbstractValidator<RevokeRefreshTokenCommand>
    {
        public RevokeRefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
            RuleFor(x => x.RequestingUserId).NotEmpty();
        }
    }

    public sealed class RevokeRefreshTokenHandler(
        IRefreshTokenStore refreshTokenStore,
        IValidator<RevokeRefreshTokenCommand> validator)
        : ICommandHandler<RevokeRefreshTokenCommand, ApiResult<bool>>
    {
        public async Task<ApiResult<bool>> Handle(RevokeRefreshTokenCommand command, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(command, ct);
            if (!validation.IsValid)
                return ApiResult<bool>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            // Ensure the token belongs to the requesting user before revoking
            var ownerResult = await refreshTokenStore.GetOwnerAsync(command.RefreshToken, ct);
            if (ownerResult is null)
                return ApiResult<bool>.Failure("Refresh token not found.");

            if (ownerResult.Value != command.RequestingUserId)
                return ApiResult<bool>.Failure("You do not own this refresh token.");

            await refreshTokenStore.RevokeAsync(command.RefreshToken, ct);
            return ApiResult<bool>.Success(true);
        }
    }
}
