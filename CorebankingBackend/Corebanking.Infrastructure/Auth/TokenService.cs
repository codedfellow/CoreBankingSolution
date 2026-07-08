using Corebanking.Application.Common;
using Corebanking.Application.Contracts.Common;
using Corebanking.Application.DTOs;
using Corebanking.Infrastructure.Configurations;
using Corebanking.Persistence.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Corebanking.Infrastructure.Auth
{
    public sealed class TokenService() : ITokenService
    {
        public (string token, DateTime expiresAtUtc) GenerateAccessToken(
        Guid userId, string email, string firstName, string lastName, IList<string> roles)
        {
            var expiresAtUtc = DateTime.UtcNow.AddMinutes(EnvironmentVariables.AccessTokenMinutes);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("first_name", firstName),
            new("last_name", lastName)
        };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.JwtSigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: EnvironmentVariables.JwtIssuer,
                audience: EnvironmentVariables.JwtAudience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
        }

        public (string token, DateTime expiresAtUtc) GenerateAccessToken(AppUserDto user, IList<string> roles)
        {
            var expiresAtUtc = DateTime.UtcNow.AddMinutes(EnvironmentVariables.AccessTokenMinutes);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("first_name", user.FirstName),
            new("last_name", user.LastName)
        };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.JwtSigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: EnvironmentVariables.JwtIssuer,
                audience: EnvironmentVariables.JwtAudience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
        }

        public string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        public ApiResult<TokenPrincipal> GetPrincipalFromExpiredToken(string accessToken)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false, // ✅ Ignore expiry intentionally
                    ValidIssuer = EnvironmentVariables.JwtIssuer,
                    ValidAudience = EnvironmentVariables.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(EnvironmentVariables.JwtSigningKey))
                };

                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    return ApiResult<TokenPrincipal>.Failure("Invalid token algorithm.");
                }

                var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub)
                    ?? principal.FindFirst(ClaimTypes.NameIdentifier);

                var emailClaim = principal.FindFirst(JwtRegisteredClaimNames.Email)
                    ?? principal.FindFirst(ClaimTypes.Email);

                if (userIdClaim is null || emailClaim is null)
                    return ApiResult<TokenPrincipal>.Failure("Token is missing required claims.");

                if (!Guid.TryParse(userIdClaim.Value, out var userId))
                    return ApiResult<TokenPrincipal>.Failure("Invalid user ID in token.");

                return ApiResult<TokenPrincipal>.Success(new TokenPrincipal(userId, emailClaim.Value));
            }
            catch (Exception ex)
            {
                return ApiResult<TokenPrincipal>.Failure($"Token validation failed: {ex.Message}");
            }
        }
    }
}
