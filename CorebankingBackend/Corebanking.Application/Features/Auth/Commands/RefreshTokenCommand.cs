using Corebanking.Application.Common;
using Corebanking.Application.Contracts;
using Corebanking.Application.Contracts.Common;
using Corebanking.Application.Contracts.CQRS;
using Corebanking.Application.Features.Auth.Dtos;
using FluentValidation;

namespace Corebanking.Application.Features.Auth.Commands
{
    public sealed record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken) : ICommand<ApiResult<AuthResponse>>;

    public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }

    public sealed class RefreshTokenHandler(
        IAuthService authService,
        ITokenService tokenService,
        IRefreshTokenStore refreshTokenStore,
        IValidator<RefreshTokenCommand> validator)
        : ICommandHandler<RefreshTokenCommand, ApiResult<AuthResponse>>
    {
        public async Task<ApiResult<AuthResponse>> Handle(RefreshTokenCommand command, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(command, ct);
            if (!validation.IsValid)
                return ApiResult<AuthResponse>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            // Validate the expired access token to extract claims — we trust the
            // signature even though the token is expired, so we only skip lifetime validation
            var principalResult = tokenService.GetPrincipalFromExpiredToken(command.AccessToken);
            if (!principalResult.IsSuccess)
                return ApiResult<AuthResponse>.Failure("Invalid access token.");

            var userId = principalResult.Value!.UserId;
            var email = principalResult.Value!.Email;

            // Validate and consume (rotate) the refresh token atomically
            var storedUserId = await refreshTokenStore.ValidateAndConsumeAsync(command.RefreshToken, ct);
            if (storedUserId is null)
                return ApiResult<AuthResponse>.Failure("Refresh token is invalid or expired.");

            // Ensure the refresh token actually belongs to the user in the access token
            if (storedUserId.Value != userId)
                return ApiResult<AuthResponse>.Failure("Token mismatch.");

            // Fetch fresh user data and roles
            var userResult = await authService.GetUserByIdAsync(userId, ct);
            if (!userResult.Succeeded || userResult.UserId is null)
                return ApiResult<AuthResponse>.Failure("User not found.");

            if (!userResult.IsActive)
                return ApiResult<AuthResponse>.Failure("Account is inactive.");

            // Issue brand new access token + refresh token (rotation)
            var (newAccessToken, expiresAtUtc) = tokenService.GenerateAccessToken(
                userResult.UserId.Value,
                userResult.Email!,
                userResult.FirstName!,
                userResult.LastName!,
                userResult.Roles!);

            var newRefreshToken = tokenService.GenerateRefreshToken();
            await refreshTokenStore.SaveAsync(userResult.UserId.Value, newRefreshToken, ct);

            return ApiResult<AuthResponse>.Success(new AuthResponse(
                userResult.UserId.Value,
                userResult.Email!,
                newAccessToken,
                expiresAtUtc,
                newRefreshToken));
        }
    }
}
