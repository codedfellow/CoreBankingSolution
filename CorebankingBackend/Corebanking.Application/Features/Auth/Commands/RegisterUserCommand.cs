
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
    public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<ApiResult<AuthResponse>>;

    public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }

    public sealed class RegisterUserHandler(
    IAuthService authService,
    ITokenService tokenService,
    IRefreshTokenStore refreshTokenStore,
    IValidator<RegisterUserCommand> validator)
    : ICommandHandler<RegisterUserCommand, ApiResult<AuthResponse>>
    {
        public async Task<ApiResult<AuthResponse>> Handle(RegisterUserCommand command, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(command, ct);
            if (!validation.IsValid)
                return ApiResult<AuthResponse>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            var ApiResult = await authService.RegisterAsync(
                command.FirstName,
                command.LastName,
                command.Email,
                command.Password,
                ct);

            if (!ApiResult.Succeeded)
                return ApiResult<AuthResponse>.Failure(ApiResult.Errors!);

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
