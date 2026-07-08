using Corebanking.API.Common;
using Corebanking.Application.Contracts.CQRS;
using Corebanking.Application.Features.Auth.Commands;
using Corebanking.Application.Features.Auth.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace Corebanking.API.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup(EndpointRoutes.BaseV1Routes).WithTags("Auth");

            group.MapPost("/register", async Task<Results<Created<AuthResponse>, BadRequest<ErrorResponse>>> (
                RegisterRequest request,
                IDispatcher dispatcher,
                CancellationToken ct) =>
            {
                var command = new RegisterUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password);

                var result = await dispatcher.Send(command, ct);

                return result.IsSuccess
                    ? TypedResults.Created($"/api/users/{result.Value!.UserId}", result.Value)
                    : TypedResults.BadRequest(new ErrorResponse(result.Errors));
            })
            .WithName("Register")
            .WithSummary("Register a new user")
            .WithDescription("Creates a new user account and returns a JWT access token");

            group.MapPost("/login", async Task<Results<Ok<AuthResponse>, UnauthorizedHttpResult, BadRequest<ErrorResponse>>> (
                    LoginRequest request,
                    IDispatcher dispatcher,
                    CancellationToken ct) =>
            {
                var command = new LoginUserCommand(request.Email, request.Password);
                var result = await dispatcher.Send(command, ct);

                if (!result.IsSuccess)
                {
                    return result.Errors.Contains("Account is locked. Try again later.")
                        ? TypedResults.BadRequest(new ErrorResponse(result.Errors))
                        : TypedResults.Unauthorized();
                }

                return TypedResults.Ok(result.Value!);
            })
            .WithName("Login")
            .WithSummary("Login")
            .WithDescription("Authenticates a user and returns a JWT access token");

            group.MapPost("/refresh-token", async Task<Results<Ok<AuthResponse>, UnauthorizedHttpResult, BadRequest<ErrorResponse>>> (
            RefreshTokenRequest request, IDispatcher dispatcher, CancellationToken ct) =>
            {
                var result = await dispatcher.Send(
                    new RefreshTokenCommand(request.AccessToken, request.RefreshToken), ct);

                if (!result.IsSuccess)
                {
                    return result.Errors.Any(e => e.Contains("Invalid") || e.Contains("mismatch") || e.Contains("expired"))
                        ? TypedResults.Unauthorized()
                        : TypedResults.BadRequest(new ErrorResponse(result.Errors));
                }

                return TypedResults.Ok(result.Value!);
            })
            .WithName("RefreshToken")
            .WithSummary("Refresh access token")
            .WithDescription("Issues a new access token and refresh token using a valid refresh token. Old refresh token is revoked (rotation).");

            group.MapPost("/revoke-token", async Task<Results<NoContent, NotFound<ErrorResponse>, ForbidHttpResult>> (
                RevokeTokenRequest request, IDispatcher dispatcher, ClaimsPrincipal user, CancellationToken ct) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? user.FindFirst("sub")?.Value;

                if (!Guid.TryParse(userIdClaim, out var userId))
                    return TypedResults.Forbid();

                var result = await dispatcher.Send(
                    new RevokeRefreshTokenCommand(request.RefreshToken, userId), ct);

                if (!result.IsSuccess)
                {
                    return result.Errors.Any(e => e.Contains("not found"))
                        ? TypedResults.NotFound(new ErrorResponse(result.Errors))
                        : TypedResults.Forbid();
                }

                return TypedResults.NoContent();
            })
            .RequireAuthorization()
            .WithName("RevokeToken")
            .WithSummary("Revoke a refresh token")
            .WithDescription("Revokes a specific refresh token. User must be authenticated.");

            group.MapPost("/revoke-all-tokens", async Task<Results<NoContent, UnauthorizedHttpResult>> (
                IDispatcher dispatcher, ClaimsPrincipal user, CancellationToken ct) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? user.FindFirst("sub")?.Value;

                if (!Guid.TryParse(userIdClaim, out var userId))
                    return TypedResults.Unauthorized();

                await dispatcher.Send(new RevokeAllRefreshTokensCommand(userId), ct);
                return TypedResults.NoContent();
            })
            .RequireAuthorization()
            .WithName("RevokeAllTokens")
            .WithSummary("Revoke all refresh tokens")
            .WithDescription("Revokes all active refresh tokens for the current user — effectively logs out from all devices.");

            group.MapGet("/me", Results<Ok<CurrentUserResponse>, UnauthorizedHttpResult> (
                    ClaimsPrincipal user) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? user.FindFirst("sub")?.Value;

                if (userId is null)
                    return TypedResults.Unauthorized();

                return TypedResults.Ok(new CurrentUserResponse(
                    userId,
                    user.FindFirst(ClaimTypes.Email)?.Value ?? user.FindFirst("email")?.Value ?? string.Empty,
                    user.FindFirst("first_name")?.Value ?? string.Empty,
                    user.FindFirst("last_name")?.Value ?? string.Empty,
                    user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()));
            })
            .RequireAuthorization()
            .WithName("GetCurrentUser")
            .WithSummary("Get current user")
            .WithDescription("Returns the currently authenticated user profile");

            return app;
        }
    }

    public sealed record RegisterRequest(string FirstName, string LastName, string Email, string Password);
    public sealed record LoginRequest(string Email, string Password);
    public sealed record RefreshTokenRequest(string AccessToken, string RefreshToken);
    public sealed record RevokeTokenRequest(string RefreshToken);

    public sealed record ErrorResponse(string[] Errors);

    public sealed record CurrentUserResponse(
        string Id,
        string Email,
        string FirstName,
        string LastName,
        IList<string> Roles);
}
