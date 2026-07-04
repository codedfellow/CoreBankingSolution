using Corebanking.Application.Common;
using Corebanking.Application.Contracts;
using Corebanking.Application.Contracts.Common;
using Corebanking.Application.Contracts.CQRS;
using Corebanking.Application.Features.Auth.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Features.Auth.Commands
{
    public sealed record LoginUserCommand(string Email, string Password) : ICommand<ApiResult<AuthResponse>>;

    public sealed class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public sealed class LoginUserHandler(
        IAuthService authService,
        ITokenService tokenService,
        IRefreshTokenStore refreshTokenStore,
        IValidator<LoginUserCommand> validator)
        : ICommandHandler<LoginUserCommand, ApiResult<AuthResponse>>
    {
        public async Task<ApiResult<AuthResponse>> Handle(LoginUserCommand command, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(command, ct);
            if (!validation.IsValid)
                return ApiResult<AuthResponse>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            var ApiResult = await authService.LoginAsync(command.Email, command.Password, ct);

            if (!ApiResult.Succeeded)
            {
                if (ApiResult.IsLockedOut)
                    return ApiResult<AuthResponse>.Failure("Account is locked. Try again later.");
                return ApiResult<AuthResponse>.Failure("Invalid credentials.");
            }

            var (accessToken, expiresAtUtc) = tokenService.GenerateAccessToken(
                ApiResult.UserId!.Value,
                ApiResult.Email!,
                ApiResult.FirstName!,
                ApiResult.LastName!,
                ApiResult.Roles!);

            var refreshToken = tokenService.GenerateRefreshToken();
            await refreshTokenStore.SaveAsync(ApiResult.UserId.Value, refreshToken, ct);

            return ApiResult<AuthResponse>.Success(new AuthResponse(
                ApiResult.UserId!.Value,
                ApiResult.Email!,
                accessToken,
                expiresAtUtc,
                refreshToken));
        }
    }
}
